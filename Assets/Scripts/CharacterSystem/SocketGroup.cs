using UnityEngine;

namespace CharacterSystem
{
    public class SocketGroup : MonoBehaviour
    {
        [SerializeField] private Transform _rightHand;
        [SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _projectile;
        
        public Transform RightHand => _rightHand;
        public Transform LeftHand => _leftHand;
        public Transform Projectile => _projectile;
    }
}