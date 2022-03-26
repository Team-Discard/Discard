using EntitySystem;

namespace CombatSystem
{
    public interface IDamageTakerComponent : IComponent<IDamageTakerComponent>
    {
        void HandleDamage(int id, in Damage damage);
        void ReckonAllDamage();
        bool Dead => false;
    }
}