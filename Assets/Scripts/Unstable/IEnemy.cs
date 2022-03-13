using EntitySystem;

namespace Unstable
{
    public interface IEnemy : ITicker
    {
        public bool Defeated => false;
    }
}