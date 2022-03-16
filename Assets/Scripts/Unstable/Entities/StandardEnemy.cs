using System.Collections.Generic;
using Animancer;
using CombatSystem;
using EntitySystem;
using UnityEngine;
using Uxt;
using Uxt.Debugging;
using WeaponSystem;
using WeaponSystem.Swords;

namespace Unstable.Entities
{
    public class StandardEnemy :
        MonoBehaviour,
        IEnemy
    {
        private IPawn _pawn;
        [SerializeField] private LocomotionController _locomotionController;
        [SerializeField] private EnemyAI _enemyAI;
        [SerializeField] private float _speed;
        [SerializeField] private StandardWeaponLocomotionAnimationSet _defaultAnimationSet;
        [SerializeField] private ClipTransition _stabAnimation;
        [SerializeField] private RootMotionFrame _rootMotionFrame;
        [SerializeField] private float _maxAngularVelocityDuringAttack;
        [SerializeField] private float _rotationThreshold;

        [SerializeField] private Sword _sword;

        [SerializeField] private HurtBox _hurtBox;

        private IHealthBar _healthBar;
        private StandardHealthModifier _healthModifier;

        private PawnAnimationHandler _animationHandler;

        private bool _attackAnimationPlayed;

        public bool Defeated { get; private set; }

        private void Awake()
        {
            Defeated = false;

            _healthBar = new StandardHealthBar(this, 4);
            Debug.Assert(_healthBar != null, "Enemy implementations must have a health bar", this);

            _pawn = new CharacterControllerPawn(this, GetComponent<CharacterController>());
            Debug.Assert(_pawn != null, "Enemy implementation must have a pawn", this);
            
            _healthModifier = new StandardHealthModifier(
                DamageLayer.Enemy,
                0.5f,
                new List<HurtBox>
                {
                    _hurtBox
                });

            _animationHandler = new PawnAnimationHandler(
                _pawn,
                GetComponentInChildren<AnimancerComponent>(),
                _defaultAnimationSet);
            
            _attackAnimationPlayed = false;
        }

        void IEntity.AddTo(IComponentRegistry registry)
        {
            registry.AddEnemy(this);
            registry.AddHealthBar(_healthBar);
            registry.AddDamageTaker(_healthModifier);
            registry.AddPawn(_pawn);
            registry.AddPawnAnimationHandler(_animationHandler);
        }
        
        public IEntity Entity => this;
        public bool Destroyed { get; private set; }

        public void Destroy()
        {
            Destroyed = true;
            Destroy(gameObject);
        }

        public void Tick(float deltaTime)
        {
            _healthBar.CurrentHealth -= _healthModifier.ConsumeAllDamage();

            if (_healthBar.CurrentHealth <= 0.0f)
            {
                Defeated = true;
                Destroy();
            }

            if (!Defeated)
            {
                _enemyAI.Tick(deltaTime);
            }

            DebugMessageManager.AddOnScreen($"Attack playing: {_enemyAI.IsMoving}", 42, Color.blue, 0.0f);

            var translationFrame = new TranslationFrame();
            var rotationFrame = _pawn.GetRotationFrame().PrepareNextFrame();

            if (!Defeated)
            {
                DoThingsAccordingToAI(deltaTime, ref translationFrame, ref rotationFrame);
            }
            
            _locomotionController.ApplyGravity(deltaTime, ref translationFrame);
            _pawn.SetTranslationFrame(translationFrame);
            _pawn.SetRotationFrame(rotationFrame);
            _animationHandler.SetAbsoluteSpeed(_pawn.CalculateForwardSpeed());
        }

        private void DoThingsAccordingToAI(float deltaTime, ref TranslationFrame translationFrame,
            ref RotationFrame rotationFrame)
        {
            if (_enemyAI.IsMoving)
            {
                var moveTowardsParams = new MoveTowardsParams
                {
                    MyPos = transform.position,
                    TargetPos = _enemyAI.PlayerTransform.position,
                    Speed = _speed
                };
                _locomotionController.MoveTowards(
                    deltaTime, moveTowardsParams,
                    ref translationFrame,
                    ref rotationFrame);
            }
            else
            {
                if (!_attackAnimationPlayed)
                {
                    _rootMotionFrame.BeginAccumulateDisplacement();
                    _attackAnimationPlayed = true;
                    var damageId = DamageManager.SetDamage(new Damage
                    {
                        BaseAmount = 1.0f,
                        DamageBox = _sword.DamageVolumes[0],
                        Layer = DamageLayer.Enemy
                    });
                    _animationHandler.PlayAnimation(
                        _stabAnimation,
                        () =>
                        {
                            _enemyAI.IsSlashing = false;
                            _attackAnimationPlayed = false;
                            _rootMotionFrame.EndAccumulateDisplacement();
                            DamageManager.ClearDamage(ref damageId);
                        });
                }
                else
                {
                    translationFrame.Displacement += _rootMotionFrame.Displacement;
                }

                var angleDiff = Vector3.SignedAngle(
                    transform.forward,
                    _enemyAI.PlayerTransform.position - transform.position,
                    Vector3.up);

                if (Mathf.Abs(angleDiff) >= _rotationThreshold)
                {
                    var deltaAngle = Mathf.MoveTowardsAngle(0.0f, angleDiff,
                        _maxAngularVelocityDuringAttack * Time.deltaTime);
                    rotationFrame.AddOverrideLinearRotation(deltaAngle);
                }
            }
        }
    }
}