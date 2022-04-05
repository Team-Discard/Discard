using Animancer;
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
        private PawnAnimationHandler _animationHandler;
        private float _windUpTimer;
        private FrameData<Translation> _translationFrame;
        private FrameData<Rotation> _rotationFrame;
        
        private RootMotionSource _rootMotionSource;
        private RootMotionFrame _rootMotionFrame;

        public bool Completed { get; private set; }
        public IReadOnlyFrameData<Translation> TranslationFrame => _translationFrame;
        public IReadOnlyFrameData<Rotation> RotationFrame => _rotationFrame;

        private void Awake()
        {
            Completed = false;
            _translationFrame = new FrameData<Translation>();
            _rotationFrame = new FrameData<Rotation>();
        }

        public void Init(DependencyBag bag)
        {
            Debug.Assert(_windUpAnimation.Clip.isLooping, "A throwing windup animation must be looping!");
            bag.ForceGet(out _animationHandler);
            bag.ForceGet(out _rootMotionSource);
        }

        public void Begin()
        {
            _windUpTimer = _windUpTime;
            _rootMotionFrame = _rootMotionSource.BeginAccumulate();
            _animationHandler.BeginPlayActionAnimation(this);
            _animationHandler.PlayActionAnimation(this, _leadAnimation, () =>
            {
                _animationHandler.PlayActionAnimation(this, _windUpAnimation);
            });
        }

        public void Finish()
        {
            _animationHandler.EndPlayActionAnimation(this);
            _rootMotionFrame.Destroy();
        }

        public void Execute(float deltaTime)
        {
            _translationFrame.Value = Translation.Identity;
            _translationFrame.UpdateValue(translation =>
            {
                translation.Displacement += _rootMotionFrame.ConsumeDeltaPosition();
                return translation;
            });
            
            
            // todo: to:billy the movement system really needs some clear documentation and refactoring
            // It is currently very hard to understand
            
            _rotationFrame.Value = Rotation.Identity;
            _rotationFrame.UpdateValue(rotation =>
            {
                rotation.Delta *= _rootMotionFrame.ConsumeDeltaRotation();
                return rotation;
            });

            if (_windUpTimer >= 0.0f)
            {
                _windUpTimer -= deltaTime;
                if (_windUpTimer <= 0.0f)
                {
                    _animationHandler.PlayActionAnimation(this, _throwAnimation, () => Completed = true);
                }
            }
        }
    }
}