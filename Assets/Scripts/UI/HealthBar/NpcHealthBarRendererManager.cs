using System;
using System.Collections.Generic;
using EntitySystem;
using UnityEngine;

namespace UI.HealthBar
{
    public class NpcHealthBarRendererManager : MonoBehaviour
    {
        [SerializeField] private HealthBarRenderer _rendererPrefab;
        private Dictionary<IHealthBarTransformComponent, HealthBarRenderer> _renderers;
        private IComponentRegistry _componentRegistry;
        private Camera _mainCamera;
        
        private void Awake()
        {
            _renderers = new Dictionary<IHealthBarTransformComponent, HealthBarRenderer>();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
        }

        public NpcHealthBarRendererManager BindComponentRegistry(IComponentRegistry registry)
        {
            _componentRegistry = registry;
            return this;
        }

        private List<IHealthBarTransformComponent> _removeBuffer = new();

        public void Tick(ComponentList<IHealthBarTransformComponent> healthBars)
        {
            SetHealthBars(healthBars);
            SyncHealthBarLocations();
        }
        
        private void SetHealthBars(ComponentList<IHealthBarTransformComponent> healthBars)
        {
            _removeBuffer.Clear();
            foreach (var (tr, rdr) in _renderers)
            {
                if (tr.Destroyed)
                {
                    rdr.Destroy();
                    _removeBuffer.Add(tr);
                }
                else
                {
                    Debug.Assert(!rdr.Destroyed, "A health bar renderer has been destroyed while still in use " +
                                                 "by a health bar renderer manager. Bug?");
                }
            }

            foreach (var key in _removeBuffer)
            {
                _renderers.Remove(key);
            }
            
            healthBars.RemoveDestroyed();
            // to:billy todo: we need a proper tick for no deltaTime use case
            healthBars.Tick(0.0f, (hb, _) => TryAddHealthBar(hb));
        }

        private void TryAddHealthBar(IHealthBarTransformComponent healthBar)
        {
            if (_renderers.ContainsKey(healthBar))
            {
                return;
            }

            var rdr = Instantiate(_rendererPrefab, transform);
            rdr.BindWatcher(healthBar.Watcher);
            Entity.SetUp(rdr.transform, _componentRegistry);
            
            // eval: some better way to do this? This couples with the health bar renderer impl. (assumes it uses a
            // camera facing canvas?)
            if (rdr.TryGetComponent(out Canvas canvas))
            {
                canvas.worldCamera = _mainCamera;
            }
            
            _renderers.Add(healthBar, rdr);
        }

        private void SyncHealthBarLocations()
        {
            foreach (var (tr, rdr) in _renderers)
            {
                rdr.transform.position = tr.Position;
                rdr.transform.LookAt(_mainCamera.transform.position);
                rdr.transform.Rotate(0,180,0);
            }
        }
    }
}