using UnityEngine;

namespace CombatSystem
{
    public static class DamageIdAllocator
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            _id = 0;
        }
        
        private static int _id;

        public static Damage CreateDamage(DamageLayer layer)
        {
            return new Damage
            {
                Id = AllocateId(),
                Layer = layer
            };
        }
        
        public static int AllocateId()
        {
            return _id++;
        }
    }
}