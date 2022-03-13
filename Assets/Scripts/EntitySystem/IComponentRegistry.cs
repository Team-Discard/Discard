using Unstable;

namespace EntitySystem
{
    public interface IComponentRegistry
    {
        private static void Empty() {}
        public void AddEnemy(IEnemy enemy) => Empty();
        public void AddDamageTaker(IDamageTaker damageTaker) => Empty();
    }
}