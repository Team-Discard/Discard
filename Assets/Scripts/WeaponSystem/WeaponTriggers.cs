using Uxt;

namespace WeaponSystem
{
    public class WeaponTriggers
    {
        public Trigger<WeaponEquipDesc> EquipTrigger { get; } = new();
        public Trigger<bool> UnEquipTrigger { get; } = new();
    }
}