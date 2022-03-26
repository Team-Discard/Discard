using EntitySystem;

namespace Unstable
{
    public interface IEnemyComponent : ITicker, IComponent<IEnemyComponent>
    {
    }
}