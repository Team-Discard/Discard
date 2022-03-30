using EntitySystem;
using UnityEngine;

namespace UI.HealthBar
{
    public interface IHealthBarTransformComponent : IComponent<IHealthBarTransformComponent>
    {
        // eval: evaluate binding ui to data values, using health bars as an example. Compare
        // the methods used in player health bar and enemy health bar.
        
        public IHealthBarWatcherComponent Watcher { get; }
        public Vector3 Position { get; }
    }
}