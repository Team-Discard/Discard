using EntitySystem;

namespace UI.HealthBar
{
    public interface IHealthBarWatcherComponent : IComponent<IHealthBarWatcherComponent>
    {
        public void Tick(float deltaTime);
        float MaxHealth { get; }
        float Health { get; }
        float HealthBeforeDamage { get; }
    }
}