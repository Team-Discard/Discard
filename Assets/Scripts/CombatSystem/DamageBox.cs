using UnityEngine;
using Unstable;
using Uxt;

namespace CombatSystem
{
    public class DamageBox : MonoBehaviour, IDamageBox
    {
        private AssociativeCounter<HurtBox> _overlappingPairsCounter;

        private void Awake()
        {
            _overlappingPairsCounter = new AssociativeCounter<HurtBox>();
        }

        public bool CheckOverlap(HurtBox hurtBox)
        {
            return _overlappingPairsCounter.CountKey(hurtBox) > 0;
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