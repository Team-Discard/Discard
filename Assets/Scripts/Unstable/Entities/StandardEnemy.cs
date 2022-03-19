using System.Collections.Generic;
using Animancer;
using CharacterSystem;
using CombatSystem;
using EntitySystem;
using UnityEngine;
using Uxt.Debugging;
using WeaponSystem;
using WeaponSystem.Swords;

namespace Unstable.Entities
{
    public class StandardEnemy :
        GameObjectComponent,
        IEnemy,
        IComponentSource
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

        public override void Init()
        {
            base.Init();

            _healthBar = new StandardHealthBar(4);
            _pawn = new CharacterControllerPawn(GetComponent<CharacterController>());
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

        public IEnumerable<IComponent> AllComponents
        {
            get
            {
                yield return _healthBar;
                yield return _pawn;
                yield return _healthModifier;
                yield return _animationHandler;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _healthBar.Destroy();
            _pawn.Destroy();
            _healthModifier.Destroy();
            _animationHandler.Destroy();
        }

        public void Tick(float deltaTime)
        {
            _healthBar.CurrentHealth -= _healthModifier.ConsumeAllDamage();

            if (_healthBar.CurrentHealth <= 0.0f)
            {
                Destroy();
            }

            if (!Destroyed)
            {
                _enemyAI.Tick(deltaTime);
            }

            DebugMessageManager.AddOnScreen($"Attack playing: {_enemyAI.IsMoving}", 42, Color.blue, 0.0f);

            var translationFrame = new TranslationFrame();
            var rotationFrame = _pawn.GetRotationFrame().PrepareNextFrame();

            if (!Destroyed)
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
                    _rootMotionFrame.BeginAccumulateDisplacement(this);
                    _attackAnimationPlayed = true;
                    var damageId = DamageManager.SetDamage(new Damage
                    {
                        BaseAmount = 1.0f,
                        DamageBox = _sword.DamageBox,
                        Layer = DamageLayer.Enemy
                    });
                    _animationHandler.PlayAnimation(
                        _stabAnimation,
                        () =>
                        {
                            _enemyAI.IsSlashing = false;
                            _attackAnimationPlayed = false;
                            _rootMotionFrame.EndAccumulateDisplacement(this);
                            DamageManager.ClearDamage(ref damageId);
                        });
                }
                else
                {
                    translationFrame.Displacement += _rootMotionFrame.ConsumeDisplacement(this);
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