using UnityEngine;

namespace Unstable
{
    public static class DamageIdAllocator
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            _id = 0;
        }
        
        private static int _id;

        private static int AllocateId()
        {
            return _id++;
        }
    }
}