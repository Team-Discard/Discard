using CombatSystem;
using UnityEngine;

namespace ProjectileSystem
{
    public class ProjectileHomingTarget : MonoBehaviour
    {
        [SerializeField] private FriendLayer _friendLayer;

        public FriendLayer FriendLayer => _friendLayer;
    }
}