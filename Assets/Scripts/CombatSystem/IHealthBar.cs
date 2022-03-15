namespace CombatSystem
{
    public interface IHealthBar
    {
        public float MaxHealth => float.PositiveInfinity;

        public float CurrentHealth
        {
            get => float.PositiveInfinity;
            set => _ = value;
        }
    }
}