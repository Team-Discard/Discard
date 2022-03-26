using EntitySystem;

namespace CombatSystem
{
    public class StandardHealthBar : IHealthBarComponent
    {
        public bool Destroyed { get; private set; }
        public float MaxHealth { get; }
        public float CurrentHealth { get; set; }

        // to:billy todo: remove dependency on damage taker. Make damage taker reference health bar instead
        public StandardHealthBar(float maxHealth, StandardDamageTaker damageTaker = null)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void Destroy()
        {
            Destroyed = true;
        }

        bool IComponent.EnabledInternal => true;

        // to:billy todo: remove this tick function

        public void Tick(float deltaTime)
        {
        }
    }
}