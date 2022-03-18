using Uxt;
using WeaponSystem.Swords;

namespace WeaponSystem
{
    public class WeaponTriggers
    {
        public DelayedMethod<SwordEquipDesc, Sword> EquipSword { get; } = new();
        public DelayedMethod<Void, bool> UnEquipAllWeapon { get; } = new();
    }
}