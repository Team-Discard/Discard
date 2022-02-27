﻿using System;
using Animancer;
using UnityEngine;
using Unstable.Entities;
using Unstable.Utils;
using WeaponSystem;
using WeaponSystem.Swords;

namespace Unstable.Actions.GreatSwordSlash
{
    public class GreatSwordSlashAction : MonoBehaviour, IAction
    {
        private PlayerPawn _playerPawn;

        [SerializeField] private ClipTransition _preparationClip;
        [SerializeField] private ClipTransition _executionClip;

        [SerializeField] private StandardWeaponLocomotionAnimationSet _locomotionAnimationSet;

        [SerializeField] private float _preparationTime;

        [SerializeField] private Sword _swordPrefab;

        private PawnAnimationHandler _animationHandler;

        private ActionStage _stage;
        private bool _preparationClipDone;
        private float _preparationTimer;
        private bool _executionClipDone;
        private bool _recoveryClipDone;

        private RootMotionFrame _rootMotionFrame;
        private WeaponTriggers _weaponTriggers;

        public void Begin()
        {
            _animationHandler.BeginPlayActionAnimation(this);
            _animationHandler.PlayActionAnimation(
                this,
                _preparationClip,
                () => { _preparationClipDone = true; });
        }

        public ActionEffects Execute(float deltaTime)
        {
            var effects = new ActionEffects
            {
                FreeMovementEnabled = false
            };
            switch (_stage)
            {
                case ActionStage.Preparation:
                {
                    effects.Interruptable = true;
                    _preparationTimer -= deltaTime;
                    if (_preparationClipDone || _preparationTimer <= 0)
                    {
                        _stage = ActionStage.Execution;
                        _animationHandler.PlayActionAnimation(
                            this,
                            _executionClip,
                            () => { _executionClipDone = true; });
                    }

                    _weaponTriggers.EquipTrigger.Set(
                        WeaponEquipDesc.EquipSword(new SwordEquipDesc
                        {
                            LocomotionAnimations = _locomotionAnimationSet,
                            SwordPrefab = _swordPrefab
                        })
                    );

                    break;
                }
                case ActionStage.Execution:
                {
                    effects.Interruptable = false;
                    effects.HorizontalVelocity += _rootMotionFrame.Velocity.ConvertXz2Xy();
                    if (_executionClipDone)
                    {
                        Completed = true;
                        _animationHandler.EndPlayActionAnimation(this);
                    }

                    break;
                }
                case ActionStage.Recovery:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return effects;
        }

        public bool Completed { get; private set; }

        public void Init(PlayerPawn pawn, PawnAnimationHandler animationHandler, WeaponTriggers weaponTriggers)
        {
            _playerPawn = pawn;
            _stage = ActionStage.Preparation;
            _preparationClipDone = false;
            _executionClipDone = false;
            _recoveryClipDone = false;
            _rootMotionFrame = _playerPawn.RootMotionFrame;
            _preparationTimer = _preparationTime;
            _animationHandler = animationHandler;
            _weaponTriggers = weaponTriggers;
        }
    }
}