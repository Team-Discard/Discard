using System;
using Unstable;

namespace CombatSystem
{
    [Serializable]
    public struct Damage
    {
        public FriendLayer Layer;
        public float BaseAmount;
        public IDamageBox DamageBox;
    }
}