using EntitySystem;

namespace CombatSystem
{
    public interface IDamageTaker : IComponent<IDamageTaker>
    {
        void HandleDamage(int id, in Damage damage);
        void ReckonAllDamage();
        bool Dead => false;
    }
}