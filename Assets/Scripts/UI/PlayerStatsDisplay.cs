using CombatSystem;
using EntitySystem;
using UI.HealthBar;
using UnityEngine;

namespace UI
{
    public class PlayerStatsDisplay : GameObjectComponent, IRegisterSelf
    {
        [SerializeField] private HealthBarRenderer _healthBarRenderer;
        public IHealthBarRendererComponent HealthBarRenderer => _healthBarRenderer;
        private HealthBarWatcher _watcher;

        public override void Init()
        {
            base.Init();
            _watcher = new HealthBarWatcher(null, 0.75f);
        }

        public void BindHealthBar(IHealthBarComponent healthBar)
        {
            _watcher.BindHealthBar(healthBar);
            _healthBarRenderer.BindWatcher(_watcher);
        }
        
        public void RegisterSelf(IComponentRegistry registry)
        {
            registry.AddComponent(_watcher);
            registry.AddComponent(_healthBarRenderer);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _watcher.Destroy();
        }
    }
}