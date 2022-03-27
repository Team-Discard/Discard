using CombatSystem;
using UI.HealthBar;
using UnityEngine;

namespace Tests.HealthBar
{
    [RequireComponent(typeof(StandardHealthBar))]
    public class ManualHealthBarTest : MonoBehaviour
    {
        [SerializeField] private GameObject _healthBarObject;
        [SerializeField] private HealthBarRenderer _renderer;
        private IHealthBarWatcherComponent _watcher;
        private IHealthBarComponent _healthBar;

        private void Start()
        {
            _healthBar = GetComponent<IHealthBarComponent>();
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