using EntitySystem;

namespace CombatSystem
{
    public class StandardHealthBar : IHealthBar
    {
        public bool Destroyed { get; private set; }
        public float MaxHealth { get; }
        public float CurrentHealth { get; set; }

        public StandardHealthBar(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void Destroy()
        {
            Destroyed = true;
        }
        
        bool IComponent.EnabledInternal => true;
    }
}