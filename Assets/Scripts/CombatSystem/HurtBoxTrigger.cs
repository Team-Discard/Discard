using UnityEngine;

namespace CombatSystem
{
    public class HurtBoxTrigger : MonoBehaviour
    {
        [SerializeField] private HurtBox _parent;
        
        public HurtBox HurtBox => _parent;
    }
}