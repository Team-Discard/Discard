using System.Collections.Generic;
using CombatSystem;
using UnityEngine;
using Unstable;

namespace WeaponSystem.Swords
{
    public sealed class Sword : MonoBehaviour
    {
        [SerializeField] private Transform _swordCenter;
        [SerializeField] private List<DamageBox> _damageVolumes;

        public Transform Center => _swordCenter;

        public IReadOnlyList<DamageBox> DamageVolumes => _damageVolumes;
    }
}