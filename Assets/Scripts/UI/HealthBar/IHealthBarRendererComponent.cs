using EntitySystem;

namespace UI.HealthBar
{
    public interface IHealthBarRendererComponent : IComponent<IHealthBarRendererComponent>
    {
        void Tick(float deltaTime);
    }
}