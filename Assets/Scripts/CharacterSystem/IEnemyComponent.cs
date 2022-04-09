using EntitySystem;
using Unstable;

namespace CharacterSystem
{
    public interface IEnemyComponent : ITicker, IComponent<IEnemyComponent>
    {
    }
}