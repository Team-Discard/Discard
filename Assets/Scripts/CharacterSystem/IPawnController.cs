using EntitySystem;
using Unstable;
using Unstable.Entities;

namespace CharacterSystem
{
    public interface IPawnController : ITicker, IComponent<IPawnController>
    {
    }
}