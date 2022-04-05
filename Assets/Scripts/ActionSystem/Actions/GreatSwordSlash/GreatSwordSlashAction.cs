using System;
using Animancer;
using CombatSystem;
using UnityEngine;
using Unstable;
using Unstable.Entities;
using Unstable.Utils;
using Uxt;
using Uxt.InterModuleCommunication;
using WeaponSystem;
using WeaponSystem.Swords;

namespace ActionSystem.Actions.GreatSwordSlash
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

        private RootMotionSource _rootMotionSource;
        private IWeaponEquipComponent _weaponEquipHandler;

        public bool Completed { get; private set; }

        private FrameData<Translation> _translationFrame;
        public IReadOnlyFrameData<Translation> TranslationFrame => _translationFrame;

        private RootMotionFrame _rootMotionFrame;

        public void Init(DependencyBag bag)
        {
            _animationHandler = bag.ForceGet<PawnAnimationHandler>();
            _weaponEquipHandler = bag.ForceGet<IWeaponEquipComponent>();
            _rootMotionSource = bag.ForceGet<RootMotionSource>();

            _stage = ActionStage.Preparation;
            _preparationClipDone = false;
            _executionClipDone = false;
            _preparationTimer = _preparationTime;
        }

        private void Awake()
        {
            _swordInstance = null;
            _translationFrame = new FrameData<Translation>();
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

        public void Execute(float deltaTime)
        {
            switch (_stage)
            {
                case ActionStage.Preparation:
                {
                    TickPreparation(deltaTime);
                    break;
                }
                case ActionStage.Execution:
                {
                    TickExecution(deltaTime);
                    break;
                }
                case ActionStage.Recovery:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TickExecution(float deltaTime)
        {
            var translation = _translationFrame.ForceReadValue();
            var displacement = _rootMotionFrame.ConsumeDeltaPosition();
            translation.Displacement = displacement;
            _translationFrame.SetValue(translation);

            if (_executionClipDone)
            {
                Completed = true;
                _animationHandler.EndPlayActionAnimation(this);
                _rootMotionFrame.Destroy();
            }
        }

        private void TickPreparation(float deltaTime)
        {
            _preparationTimer -= deltaTime;
            if (_preparationClipDone || _preparationTimer <= 0)
            {
                _stage = ActionStage.Execution;
                _animationHandler.PlayActionAnimation(
                    this,
                    _executionClip,
                    () => { _executionClipDone = true; });
                _rootMotionFrame = _rootMotionSource.BeginAccumulate();
            }

            _swordInstance = _weaponEquipHandler.EquipSword(new SwordEquipDesc
            {
                LocomotionAnimations = _locomotionAnimationSet,
                SwordPrefab = _swordPrefab
            });
            _swordDamageId = DamageManager.SetDamage(new Damage
            {
                BaseAmount = 47,
                DamageBox = _swordInstance.DamageBox
            });
        }
    }
}