using System.Collections.Generic;
using UnityEngine;
using Uxt;

namespace Unstable
{
    public class DamageVolume : MonoBehaviour, IDamageVolume
    {
        public int DamageId { get; set; }

        private AssociativeCounter<DamageablePart> _overlappingPairsCounter;

        private void Awake()
        {
            _overlappingPairsCounter = new AssociativeCounter<DamageablePart>();
        }

        public bool CheckOverlap(DamageablePart damageablePart)
        {
            return _overlappingPairsCounter.CountKey(damageablePart) > 0;
        }

        public float GetDamageAmount(DamageablePart damageablePart)
        {
            return 42.0f;
        }

        public void NotifyEnteredOverlapWith(DamageablePart damageablePart)
        {
            _overlappingPairsCounter.IncrementKey(damageablePart);
        }

        public void NotifyExitedOverlapWith(DamageablePart damageablePart)
        {
            _overlappingPairsCounter.DecrementKey(damageablePart);
        }
    }
}