using EntitySystem;
using Unstable;
using WeaponSystem.Swords;

namespace WeaponSystem
{
    public interface IWeaponEquipComponent : IComponent<IWeaponEquipComponent>, ITicker
    {
        Sword EquipSword(SwordEquipDesc parameters);
    }
}