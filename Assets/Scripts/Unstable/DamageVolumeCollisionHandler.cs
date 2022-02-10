using UnityEngine;

namespace Unstable
{
    public class DamageVolumeCollisionHandler : MonoBehaviour
    {
        [SerializeField] private DamageVolume _parent;

        private void OnTriggerEnter(Collider other)
        {
            var damageablePart = GetDamageablePartOfCollider(other);
            if (damageablePart != null)
            {
                _parent.NotifyEnteredOverlapWith(damageablePart);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var damageablePart = GetDamageablePartOfCollider(other);
            if (damageablePart != null)
            {
                _parent.NotifyExitedOverlapWith(damageablePart);
            }
        }

        private static DamageablePart GetDamageablePartOfCollider(Collider collider)
        {
            if (collider.TryGetComponent(out DamageablePartCollisionHandler damageablePartCollisionHandler))
            {
                return damageablePartCollisionHandler.DamageablePart;
            }

            return null;
        }
    }
}