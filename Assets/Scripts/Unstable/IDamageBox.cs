using CombatSystem;

namespace Unstable
{
    public interface IDamageBox
    {
        bool CheckOverlap(HurtBox hurtBox);
        Damage GetDamage();
    }
}