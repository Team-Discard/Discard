using EntitySystem;

namespace Unstable
{
    public interface IEnemy : ITicker, IComponent
    {
        public bool Defeated => false;
    }
}