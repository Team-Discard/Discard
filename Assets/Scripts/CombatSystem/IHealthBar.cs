using System;
using EntitySystem;

namespace CombatSystem
{
    public interface IHealthBar : IComponent
    {
        public float MaxHealth => float.PositiveInfinity;

        public float CurrentHealth
        {
            get => float.PositiveInfinity;
            set => _ = value;
        }

        Type IComponent.GetComponentType() => typeof(IHealthBar);
    }
}