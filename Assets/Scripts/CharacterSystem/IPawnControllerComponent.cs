using EntitySystem;
using Unstable;

namespace CharacterSystem
{
    public interface IPawnControllerComponent : ITicker, IComponent<IPawnControllerComponent>
    {
    }
}