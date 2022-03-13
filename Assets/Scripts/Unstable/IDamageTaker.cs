using CombatSystem;

namespace Unstable
{
    public interface IDamageTaker
    {
        void HandleDamage(int id, in Damage damage);
        void ReckonAllDamage();
        bool Dead => false;
    }
}