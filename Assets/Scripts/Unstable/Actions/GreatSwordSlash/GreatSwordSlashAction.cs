using System;
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

        private Sword _swordInstance;
        private PawnAnimationHandler _animationHandler;

        private ActionStage _stage;
        private bool _preparationClipDone;
        private float _preparationTimer;
        private bool _executionClipDone;

        private RootMotionFrame _rootMotionFrame;
        private WeaponTriggers _weaponTriggers;

        public bool Completed { get; private set; }

        public void Init(PlayerPawn pawn, PawnAnimationHandler animationHandler, WeaponTriggers weaponTriggers)
        {
            _playerPawn = pawn;
            _stage = ActionStage.Preparation;
            _preparationClipDone = false;
            _executionClipDone = false;
            _rootMotionFrame = _playerPawn.RootMotionFrame;
            _preparationTimer = _preparationTime;
            _animationHandler = animationHandler;
            _weaponTriggers = weaponTriggers;
        }

        private void Awake()
        {
            _swordInstance = null;
        }

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
                    TickPreparation(deltaTime, ref effects);
                    break;
                }
                case ActionStage.Execution:
                {
                    TickExecution(deltaTime, ref effects);
                    break;
                }
                case ActionStage.Recovery:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return effects;
        }

        private void TickExecution(float deltaTime, ref ActionEffects effects)
        {
            effects.Interruptable = false;
            effects.HorizontalVelocity += _rootMotionFrame.Velocity.ConvertXz2Xy();

            if (_swordInstance != null)
            {
                effects.DamageVolumes = _swordInstance.DamageVolumes;
            }

            if (_executionClipDone)
            {
                Completed = true;
                _animationHandler.EndPlayActionAnimation(this);
            }
        }

        private void TickPreparation(float deltaTime, ref ActionEffects effects)
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

            _weaponTriggers.EquipSword.InvokeDelayed(
                new SwordEquipDesc
                {
                    LocomotionAnimations = _locomotionAnimationSet,
                    SwordPrefab = _swordPrefab
                },
                sword => { _swordInstance = sword; });
        }
    }
}