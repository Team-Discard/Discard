using System.Collections.Generic;
using CombatSystem;
using UnityEngine;
using Unstable;

namespace WeaponSystem.Swords
{
    public sealed class Sword : MonoBehaviour
    {
        [SerializeField] private Transform _swordCenter;
        [SerializeField] private DamageBox _damageBox;
        [SerializeField] private StandardWeaponLocomotionAnimationSet _locomotionAnimation;
        
        public Transform Center => _swordCenter;
        public DamageBox DamageBox => _damageBox;
        public StandardWeaponLocomotionAnimationSet LocomotionAnimations => _locomotionAnimation;
    }
}