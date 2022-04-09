using Animancer;
using CameraSystem;
using CharacterSystem;
using CombatSystem;
using EntitySystem;
using MotionSystem;
using UI.HealthBar;
using UnityEngine;
using UnityEngine.Serialization;
using Unstable.Utils;
using Uxt.Debugging;
using WeaponSystem;
using WeaponSystem.Swords;

namespace Unstable.Entities
{
    [RequireComponent(typeof(StandardHealthBar))]
    public class StandardEnemy :
        GameObjectComponent,
        IEnemyComponent,
        IRegisterSelf
    {
        private IPawnComponent _pawn;
        [SerializeField] private LocomotionController _locomotionController;
        [SerializeField] private EnemyAI _enemyAI;
        [SerializeField] private float _speed;
        [SerializeField] private StandardWeaponLocomotionAnimationSet _defaultAnimationSet;
        [SerializeField] private ClipTransition _stabAnimation;

        [FormerlySerializedAs("_rootMotionFrame")] [SerializeField]
        private RootMotionSource rootMotionSource;

        [SerializeField] private float _maxAngularVelocityDuringAttack;
        [SerializeField] private float _rotationThreshold;
        [SerializeField] private Sword _sword;
        private StandardHealthBar _healthBar;
        private EnemyHealthBarTransform _healthBarTransform;
        [SerializeField] private StandardDamageTaker _damageTaker;

        [SerializeField] private Transform _lockOnPoint;
        
        private RootMotionFrame _rootMotionFrame;
        private PawnAnimationHandler _animationHandler;
        private bool _attackAnimationPlayed;

        private int _damageId;
        
        public override void Init()
        {
            base.Init();

            _damageId = -1;
            
            _healthBar = GetComponent<StandardHealthBar>();
            _damageTaker.BindHealthBar(_healthBar);

            _pawn = new CharacterControllerPawn(GetComponent<CharacterController>());
            _animationHandler = new PawnAnimationHandler(
                _pawn,
                GetComponentInChildren<AnimancerComponent>(),
                _defaultAnimationSet);

            _healthBarTransform = new EnemyHealthBarTransform(transform, _healthBar);

            _attackAnimationPlayed = false;
        }

        private void Start()
        {
            LockOnTargetManager.RegisterTarget(_lockOnPoint);
        }

        public void RegisterSelf(IComponentRegistry registry)
        {
            registry.AddComponent(this);
            registry.AddComponent(_damageTaker);

            _healthBarTransform.RegisterSelf(registry);
            registry.AddComponent(_healthBar);
            registry.AddComponent(_pawn);
            registry.AddComponent(_animationHandler);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _healthBarTransform.Destroy();
            _healthBar.Destroy();
            _pawn.Destroy();
            _animationHandler.Destroy();

            if (_damageId != -1)
            {
                DamageManager.ClearDamage(ref _damageId);
            }
            
            LockOnTargetManager.UnregisterTarget(_lockOnPoint);
        }

        public void Tick(float deltaTime)
        {
            if (_healthBar.CurrentHealth <= 0.0f)
            {
                Destroy();
            }

            if (!Destroyed)
            {
                _enemyAI.Tick(deltaTime);
            }

            DebugMessageManager.AddOnScreen($"Attack playing: {_enemyAI.IsMoving}", 42, Color.blue, 0.0f);

            var translationFrame = Translation.Identity;
            var rotationFrame = Rotation.Identity;

            if (!Destroyed)
            {
                DoThingsAccordingToAI(deltaTime, ref translationFrame, ref rotationFrame);
            }

            _locomotionController.ApplyGravity(deltaTime, ref translationFrame);
            _pawn.SetTranslation(translationFrame);
            _pawn.SetRotation(rotationFrame);
            _animationHandler.SetAbsoluteSpeed(_pawn.CalculateForwardSpeed());
        }

        private void DoThingsAccordingToAI(float deltaTime, ref Translation translation,
            ref Rotation rotation)
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
                    deltaTime, transform.forward.ConvertXz2Xy(), 120.0f, moveTowardsParams,
                    ref translation,
                    ref rotation);
            }
            else
            {
                if (!_attackAnimationPlayed)
                {
                    _rootMotionFrame = rootMotionSource.BeginAccumulate();
                    _attackAnimationPlayed = true;
                    _damageId = DamageManager.SetDamage(new Damage
                    {
                        BaseAmount = 1.0f,
                        DamageBox = _sword.DamageBox,
                        Layer = FriendLayer.Enemy
                    }, _damageId);
                    _animationHandler.PlayAnimation(
                        _stabAnimation,
                        () =>
                        {
                            _enemyAI.IsSlashing = false;
                            _attackAnimationPlayed = false;
                            _rootMotionFrame.Destroy();
                            
                            // todo: to:billy we need a RAII interface for damage IDs (expressing damage ownership)
                            DamageManager.ClearDamage(ref _damageId);
                        });
                }
                else
                {
                    translation.Displacement += _rootMotionFrame.ConsumeDeltaPosition();
                }

                var angleDiff = Vector3.SignedAngle(
                    transform.forward,
                    _enemyAI.PlayerTransform.position - transform.position,
                    Vector3.up);

                if (Mathf.Abs(angleDiff) >= _rotationThreshold)
                {
                    var deltaAngle = Mathf.MoveTowardsAngle(0.0f, angleDiff,
                        _maxAngularVelocityDuringAttack * deltaTime);
                    rotation.YawVelocity += deltaAngle / deltaTime;
                }
            }
        }
    }
}