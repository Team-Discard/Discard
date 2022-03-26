using EntitySystem;
using Unstable;

namespace CombatSystem
{
    public interface IDeathCheckComponent : IComponent<IDeathCheckComponent>, ITicker
    {
    }
}