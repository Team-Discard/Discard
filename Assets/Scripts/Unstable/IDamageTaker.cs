using CombatSystem;

namespace Unstable
{
    public interface IDamageTaker
    {
        void HandleDamage(IDamageBox damageBox);
        void ReckonAllDamage(float deltaTime);
    }
}