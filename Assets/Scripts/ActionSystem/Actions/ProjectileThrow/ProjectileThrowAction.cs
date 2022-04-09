using Animancer;
using CharacterSystem;
using CombatSystem;
using EntitySystem;
using MotionSystem;
using ProjectileSystem;
using UnityEngine;
using Unstable.Entities;
using Uxt.InterModuleCommunication;

namespace ActionSystem.Actions.ProjectileThrow
{
    public class ProjectileThrowAction : MonoBehaviour, IAction
    {
        [SerializeField] private float _windUpTime;
        [SerializeField] private ClipTransition _leadAnimation;
        [SerializeField] private ClipTransition _windUpAnimation;
        [SerializeField] private ClipTransition _throwAnimation;
        [SerializeField] private ProjectileComponent _projectilePrefab;
        
        private PawnAnimationHandler _animationHandler;
        private float _windUpTimer;
        private FrameData<Translation> _translationFrame;
        private FrameData<Rotation> _rotationFrame;

        private SocketGroup _socketGroup;

        private RootMotionSource _rootMotionSource;
        private RootMotionFrame _rootMotionFrame;

        private ProjectileComponent _projectile;

        private FollowConstraintComponent _windUpFollowConstraint;
        private FollowConstraintComponent _actFollowConstraint;

        private DependencyBag _dependencyBag;
        
        public bool Completed { get; private set; }
        public IReadOnlyFrameData<Translation> TranslationFrame => _translationFrame;
        public IReadOnlyFrameData<Rotation> RotationFrame => _rotationFrame;

        private void Awake()
        {
            Completed = false;
            _translationFrame = new FrameData<Translation>();
            _rotationFrame = new FrameData<Rotation>();
            _windUpFollowConstraint = null;
            _actFollowConstraint = null;
        }

        public void Init(DependencyBag bag)
        {
            Debug.Assert(_windUpAnimation.Clip.isLooping, "A throwing windup animation must be looping!");

            _dependencyBag = bag;
            
            bag.ForceGet(out _animationHandler);
            bag.ForceGet(out _rootMotionSource);
            bag.ForceGet(out _socketGroup);
        }

        public void Begin()
        {
            _windUpTimer = _windUpTime;
            _rootMotionFrame = _rootMotionSource.BeginAccumulate();
            _animationHandler.BeginPlayActionAnimation(this);

            _throwAnimation.Events.AddCallback("Projectile Threw", () =>
            {
                _actFollowConstraint?.Destroy();
                _windUpFollowConstraint?.Destroy();

                var rigidBody = _projectile.GetComponent<Rigidbody>();
                rigidBody.isKinematic = false;
                rigidBody.useGravity = false;
                rigidBody.AddForce(_rootMotionSource.transform.forward * 10.0f, ForceMode.Impulse);

                _projectile.EnableHoming = true;
            });
            
            _animationHandler.PlayActionAnimation(this, _leadAnimation, () =>
            {
                _animationHandler.PlayActionAnimation(this, _windUpAnimation);
                _projectile = Instantiate(_projectilePrefab, _socketGroup.Projectile.position, Quaternion.identity);
                Entity.SetUp(_projectile.transform, ComponentRegistry.Instance);

                if (_dependencyBag.Get(out Collider throwerCollider))
                {
                    _projectile.IgnoreCollisionWith(throwerCollider);
                }
                
                // todo: this damage layer should be a parameter
                _projectile.BindDamageLayer(FriendLayer.Player);
                
                _windUpFollowConstraint = new FollowConstraintComponent(
                    ComponentRegistry.Instance,
                    _projectile.transform,
                    _socketGroup.Projectile);
                
                // todo: to:billy we need a proper audio manager
                GetComponent<AudioSource>().Play();
            });
        }

        public void Finish()
        {
            _animationHandler.EndPlayActionAnimation(this);
            _rootMotionFrame.Destroy();
            _actFollowConstraint.Destroy();
        }

        public void Execute(float deltaTime)
        {
            _translationFrame.Value = Translation.Identity;
            _translationFrame.UpdateValue(translation =>
            {
                translation.Displacement += _rootMotionFrame.ConsumeDeltaPosition();
                return translation;
            });

            _rotationFrame.Value = Rotation.Identity;
            _rotationFrame.UpdateValue(rotation =>
            {
                rotation.Delta *= _rootMotionFrame.ConsumeDeltaRotation();
                return rotation;
            });

            _windUpTimer -= deltaTime;
            if (_windUpTimer <= 0.0f && _windUpFollowConstraint != null && _actFollowConstraint == null)
            {
                _windUpFollowConstraint.Destroy();
                _animationHandler.PlayActionAnimation(this, _throwAnimation, () => Completed = true);
                _actFollowConstraint = new FollowConstraintComponent(
                    ComponentRegistry.Instance,
                    _projectile.transform, _socketGroup.RightHand);
            }
        }
    }
}