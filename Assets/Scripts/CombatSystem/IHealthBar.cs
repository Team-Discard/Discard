using System;
using EntitySystem;
using Unstable;

namespace CombatSystem
{
    public interface IHealthBar : IComponent, ITicker
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