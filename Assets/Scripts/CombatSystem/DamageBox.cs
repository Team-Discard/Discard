using UnityEngine;
using Unstable;
using Uxt;

namespace CombatSystem
{
    public class DamageBox : MonoBehaviour, IDamageBox
    {
        [SerializeField] private Damage _damage;
        public Damage Damage => _damage;

        private AssociativeCounter<HurtBox> _overlappingPairsCounter;

        private void Awake()
        {
            _damage.Id = DamageIdAllocator.AllocateId();
            _overlappingPairsCounter = new AssociativeCounter<HurtBox>();
        }

        public bool CheckOverlap(HurtBox hurtBox)
        {
            return _overlappingPairsCounter.CountKey(hurtBox) > 0;
        }

        public Damage GetDamage()
        {
            return Damage;
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