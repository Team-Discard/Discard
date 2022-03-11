using System;

namespace CombatSystem
{
    [Serializable]
    public struct Damage
    {
        public int Id { get; set; }
        public DamageLayer Layer;
        public float BaseAmount;
    }
}