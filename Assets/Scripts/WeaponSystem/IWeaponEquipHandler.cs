using EntitySystem;
using Unstable;
using WeaponSystem.Swords;

namespace WeaponSystem
{
    public interface IWeaponEquipHandler : IComponent<IWeaponEquipHandler>, ITicker
    {
        Sword EquipSword(SwordEquipDesc parameters);
    }
}