using CombatSystem;
using EntitySystem;
using UnityEngine;

namespace UI.HealthBar
{
    public class EnemyHealthBarTransform : 
        StandardComponent, 
        IHealthBarTransformComponent,
        IRegisterSelf
    {
        private readonly Transform _transform;
        public IHealthBarWatcherComponent Watcher { get; private set; }
        
        // to:billy find a proper way to implement positioning (rather that adding a fixed height)
        public Vector3 Position => _transform.position + Vector3.up * 2.0f;

        public EnemyHealthBarTransform(Transform transform, IHealthBarComponent healthBar)
        {
            _transform = transform;
            Watcher = new HealthBarWatcher(healthBar);
        }

        public void BindHealthBar(IHealthBarComponent healthBar)
        {
            Watcher?.Destroy();
            Watcher = new HealthBarWatcher(healthBar);
        }

        public void RegisterSelf(IComponentRegistry registry)
        {
            Debug.Assert(Watcher != null);
            
            registry.AddComponent(this);
            registry.AddComponent(Watcher);
        }

        public override void Destroy()
        {
            base.Destroy();
            Watcher?.Destroy();
        }
    }
}