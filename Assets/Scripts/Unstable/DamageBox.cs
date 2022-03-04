using System.Collections.Generic;
using UnityEngine;
using Uxt;

namespace Unstable
{
    public class DamageBox : MonoBehaviour, IDamageBox
    {
        public int DamageId { get; set; }

        private AssociativeCounter<HurtBox> _overlappingPairsCounter;

        private void Awake()
        {
            _overlappingPairsCounter = new AssociativeCounter<HurtBox>();
        }

        public bool CheckOverlap(HurtBox hurtBox)
        {
            return _overlappingPairsCounter.CountKey(hurtBox) > 0;
        }

        public float GetDamageAmount(HurtBox hurtBox)
        {
            return 42.0f;
        }

        public void NotifyEnteredOverlapWith(HurtBox hurtBox)
        {
            _overlappingPairsCounter.IncrementKey(hurtBox);
        }

        public void NotifyExitedOverlapWith(HurtBox hurtBox)
        {
            _overlappingPairsCounter.DecrementKey(hurtBox);
        }
    }
}