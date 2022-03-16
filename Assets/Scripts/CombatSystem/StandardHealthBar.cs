using EntitySystem;
using UnityEngine;

namespace CombatSystem
{
    public class StandardHealthBar : IHealthBar
    {
        public IEntity Entity { get; }
        public float MaxHealth { get; }
        public float CurrentHealth { get; set; }

        public StandardHealthBar(IEntity entity, float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            Entity = entity;
        }
    }
}