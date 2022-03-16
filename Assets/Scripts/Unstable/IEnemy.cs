using EntitySystem;

namespace Unstable
{
    public interface IEnemy : ITicker, IEntity, IComponent
    {
        public bool Defeated => false;
    }
}