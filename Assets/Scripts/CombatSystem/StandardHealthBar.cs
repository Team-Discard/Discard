using EntitySystem;

namespace CombatSystem
{
    public class StandardHealthBar : IHealthBar
    {
        public bool Destroyed { get; private set; }
        public float MaxHealth { get; }
        public float CurrentHealth { get; set; }

        private readonly StandardDamageTaker _damageTaker;

        public StandardHealthBar(float maxHealth, StandardDamageTaker damageTaker)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            _damageTaker = damageTaker;
        }

        public void Destroy()
        {
            Destroyed = true;
        }

        bool IComponent.EnabledInternal => true;

        public void Tick(float deltaTime)
        {
            while (_damageTaker.TryDequeueDamage(out var dmg))
            {
                CurrentHealth -= dmg.BaseAmount;
            }
        }
    }
}