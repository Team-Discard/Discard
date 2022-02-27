namespace WeaponSystem
{
    public struct WeaponEquipDesc
    {
        public SwordEquipDesc? Sword { get; set; }

        public static WeaponEquipDesc EquipSword(SwordEquipDesc swordEquipDesc)
        {
            return new WeaponEquipDesc { Sword = swordEquipDesc };
        }
    }
}