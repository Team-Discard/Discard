using System.Collections.Generic;
using UnityEngine;
using Unstable;

namespace WeaponSystem.Swords
{
    public sealed class Sword : MonoBehaviour
    {
        [SerializeField] private Transform _swordCenter;
        [SerializeField] private List<DamageVolume> _damageVolumes;

        public Transform Center => _swordCenter;

        public IReadOnlyList<DamageVolume> DamageVolumes => _damageVolumes;
    }
}