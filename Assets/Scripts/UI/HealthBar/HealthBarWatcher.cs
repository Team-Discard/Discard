using CombatSystem;
using EntitySystem;

namespace UI.HealthBar
{
    public class HealthBarWatcher : StandardComponent, IHealthBarWatcherComponent
    {
        private IHealthBarComponent _healthBar;
        private readonly float _damageStickyDelay;
        private float _healthBeforeDamage;
        private float _damageStickyTimer;
        private float _healthLastFrame;

        public HealthBarWatcher(IHealthBarComponent healthBar, float damageStickyDelay = 0.75f)
        {
            _healthBar = healthBar;
            _damageStickyDelay = damageStickyDelay;
            if (healthBar != null)
            {
                BindHealthBar(healthBar);
            }
        }

        public void BindHealthBar(IHealthBarComponent healthBar)
        {
            _healthBar = healthBar;
            _healthBeforeDamage = healthBar.CurrentHealth;
            _healthLastFrame = healthBar.CurrentHealth;
        }
        
        public void Tick(float deltaTime)
        {
            // In the case that the health bar has taken damage, reset the damage timer
            if (_healthBar.CurrentHealth < _healthLastFrame - 1e-4)
            {
                _damageStickyTimer = _damageStickyDelay;
            }

            if (_damageStickyTimer <= 0.0f)
            {
                _healthBeforeDamage = _healthBar.CurrentHealth;
            }
            else
            {
                _damageStickyTimer -= deltaTime;
            }
            
            _healthLastFrame = _healthBar.CurrentHealth;
        }

        public float MaxHealth => _healthBar.MaxHealth;
        public float Health => _healthBar.CurrentHealth;
        public float HealthBeforeDamage => _healthBeforeDamage;
    }
}