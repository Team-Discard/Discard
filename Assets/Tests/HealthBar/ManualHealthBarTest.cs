using System;
using CombatSystem;
using EntitySystem;
using UI.HealthBar;
using UnityEngine;

namespace Tests.HealthBar
{
    public class ManualHealthBarTest : MonoBehaviour
    {
        [SerializeField] private GameObject _healthBarObject;
        [SerializeField] private HealthBarRenderer _renderer;
        private IHealthBarWatcherComponent _watcher;
        private IHealthBarComponent _healthBar;

        private void Start()
        {
            _healthBar = new StandardHealthBar(120.0f);
            _watcher = new HealthBarWatcher(_healthBar, 0.75f);
            _renderer.BindWatcher(_watcher);

            Debug.Assert(_renderer != null);
        }

        public void TakeDamage(float amount)
        {
            _healthBar.CurrentHealth -= amount;
        }
        
        private void Update()
        {
            _watcher.Tick(Time.deltaTime);
            _renderer.Tick(Time.deltaTime);
        }
    }
}