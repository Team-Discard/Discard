using UnityEngine;

namespace CombatSystem
{
    
    // todo: to:billy research: does damage box trigger need rigidbody if hurt box already has that?
    
    public class DamageBoxTrigger : MonoBehaviour
    {
        [SerializeField] private DamageBox _parent;

        private void OnTriggerEnter(Collider other)
        {
            var hurtBox = GetDamageablePartOfCollider(other);
            if (hurtBox != null)
            {
                _parent.NotifyEnteredOverlapWith(hurtBox);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var hurtBox = GetDamageablePartOfCollider(other);
            if (hurtBox != null)
            {
                _parent.NotifyExitedOverlapWith(hurtBox);
            }
        }

        private static HurtBox GetDamageablePartOfCollider(Collider collider)
        {
            if (collider.TryGetComponent(out HurtBoxTrigger damageablePartCollisionHandler))
            {
                return damageablePartCollisionHandler.HurtBox;
            }

            return null;
        }
    }
}