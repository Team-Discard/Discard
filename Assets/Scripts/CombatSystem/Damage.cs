using System;
using Unstable;

namespace CombatSystem
{
    [Serializable]
    public struct Damage
    {
        public DamageLayer Layer;
        public float BaseAmount;
        public IDamageBox DamageBox;
    }
}