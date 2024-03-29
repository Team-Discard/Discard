﻿using EntitySystem;
using UnityEngine;
using Unstable.Entities;
using Uxt;
using Uxt.Utils;
using WeaponSystem.Swords;

namespace WeaponSystem
{
    public class StandardWeaponEquipHandler : StandardComponent, IWeaponEquipComponent
    {
        /*
         * to:billy | review: as a hack we use the prefab to uniquely identify different weapons
         * (so that we do not equip the same weapon twice).
         * Later we need a proper system to do this.
         */
        private Object _lastEquippedPrefab;

        private Sword _equippedSword;

        private readonly Transform _swordHandleBottom;
        private readonly Transform _swordHandleTop;
        private readonly PawnAnimationHandler _pawnAnimationHandler;

        public StandardWeaponEquipHandler(Transform swordHandleBottom, Transform swordHandleTop,
            PawnAnimationHandler animationHandler)
        {
            _swordHandleBottom = swordHandleBottom;
            _swordHandleTop = swordHandleTop;
            _pawnAnimationHandler = animationHandler;
        }

        public Sword EquipSword(SwordEquipDesc parameters)
        {
            Debug.Assert(parameters.SwordPrefab != null);
            if (parameters.SwordPrefab == _lastEquippedPrefab)
            {
                Debug.Assert(_equippedSword != null);
                return _equippedSword;
            }

            if (_equippedSword != null)
            {
                Object.Destroy(_equippedSword.gameObject);
            }

            _equippedSword = Object.Instantiate(parameters.SwordPrefab);
            _lastEquippedPrefab = parameters.SwordPrefab;
            
            _pawnAnimationHandler.SetLocomotionAnimations(_equippedSword.LocomotionAnimations);

            return _equippedSword;
        }

        public void Tick(float deltaTime)
        {
            if (_equippedSword != null)
            {
                MatchSwordToHandPosition(_equippedSword);
            }
        }

        private void MatchSwordToHandPosition(Sword sword)
        {
            var swordTransform = sword.transform;
            var swordCenterTransform = sword.Center;
            var target = IkUtility.MatchStickWithHandlePoints(_swordHandleBottom, _swordHandleTop);
            var swordTransformData = IkUtility.MoveParentToMatchChild(swordTransform, swordCenterTransform, target);
            swordTransformData.ApplyTo(swordTransform);
        }
    }
}