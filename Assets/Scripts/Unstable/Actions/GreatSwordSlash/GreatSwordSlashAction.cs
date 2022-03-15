using System;
using Animancer;
using CombatSystem;
using UnityEngine;
using Unstable.Entities;
using Unstable.Utils;
using Uxt;
using WeaponSystem;
using WeaponSystem.Swords;

namespace Unstable.Actions.GreatSwordSlash
{
    public class GreatSwordSlashAction : MonoBehaviour, IAction
    {
        [SerializeField] private ClipTransition _preparationClip;
        [SerializeField] private ClipTransition _executionClip;
        [SerializeField] private StandardWeaponLocomotionAnimationSet _locomotionAnimationSet;
        [SerializeField] private float _preparationTime;
        [SerializeField] private Sword _swordPrefab;

        private Sword _swordInstance;
        private int _swordDamageId;
        private PawnAnimationHandler _animationHandler;

        private ActionStage _stage;
        private bool _preparationClipDone;
        private float _preparationTimer;
        private bool _executionClipDone;

        private RootMotionFrame _rootMotionFrame;
        private WeaponTriggers _weaponTriggers;

        public bool Completed { get; private set; }

        public void Init(DependencyBag bag)
        {
            bag.Get(out _animationHandler);
            bag.Get(out _weaponTriggers);
            _rootMotionFrame = bag.ForceGet<RootMotionFrame>();

            _stage = ActionStage.Preparation;
            _preparationClipDone = false;
            _executionClipDone = false;
            _preparationTimer = _preparationTime;
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

        public void Finish()
        {
            if (_swordInstance != null)
            {
                Debug.Assert(_swordDamageId != -1);

                _swordInstance = null;
                DamageManager.ClearDamage(ref _swordDamageId);
            }
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
                sword =>
                {
                    _swordInstance = sword;
                    _swordDamageId = DamageManager.SetDamage(new Damage
                    {
                        BaseAmount = 47,
                        // todo: change this to dynamically bind to sword damage volumes
                        DamageBox = sword.DamageVolumes[0]
                    });
                });
        }
    }
}