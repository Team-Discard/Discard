using EntitySystem;
using UnityEngine;

namespace UI.HealthBar
{
    public class HealthBarRenderer :
        GameObjectComponent,
        IHealthBarRendererComponent,
        IRegisterSelf
    {
        [SerializeField] private RectTransform _redBar;
        [SerializeField] private RectTransform _whiteBar;
        private IHealthBarWatcherComponent _healthBarWatcher;
        private float _actualWhiteTip;

        private void Awake()
        {
            _healthBarWatcher = null;
            _actualWhiteTip = 1.0f;
        }

        public void BindWatcher(IHealthBarWatcherComponent watcher)
        {
            Debug.Assert(_healthBarWatcher == null, "_healthBarWatcher == null");
            _healthBarWatcher = watcher;
        }

        public void Tick(float deltaTime)
        {
            Debug.Assert(_healthBarWatcher != null, "You must bind a health bar watcher to this health bar renderer!",
                gameObject);

            if (_healthBarWatcher.Destroyed)
            {
                Destroy();
            }

            UpdateUI(deltaTime, _healthBarWatcher.MaxHealth, _healthBarWatcher.Health, _healthBarWatcher.HealthBeforeDamage);
        }

        private void UpdateUI(float deltaTime, float maxHealth, float health, float healthBeforeDamage)
        {
            CalculateRedAndWhiteTips(maxHealth, health, healthBeforeDamage, out var redTip, out var whiteTip);
            _actualWhiteTip = Mathf.MoveTowards(_actualWhiteTip, whiteTip, deltaTime * 0.5f);
            _whiteBar.gameObject.SetActive(Mathf.Abs(redTip - _actualWhiteTip) > 0.01f);
            _redBar.anchorMin = Vector2.zero;
            _redBar.anchorMax = new Vector2(redTip, 1.0f);
            _whiteBar.anchorMin = new Vector2(redTip, 0.0f);
            _whiteBar.anchorMax = new Vector2(_actualWhiteTip, 1.0f);
        }

        private void CalculateRedAndWhiteTips(float maxHealth, float health, float healthBeforeDamage, out float redTip,
            out float whiteTip)
        {
            redTip = Mathf.Clamp01(health / maxHealth);
            whiteTip = Mathf.Clamp01(healthBeforeDamage / maxHealth);
        }

        public void RegisterSelf(IComponentRegistry registry)
        {
            registry.AddComponent(this);
        }
    }
}