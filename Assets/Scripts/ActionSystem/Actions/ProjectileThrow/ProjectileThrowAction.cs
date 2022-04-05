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

        public bool Completed { get; private set; }
        public IReadOnlyFrameData<Translation> TranslationFrame => _translationFrame;
        
        private void Awake()
        {
            Completed = false;
            _translationFrame = new FrameData<Translation>();
        }

        public void Init(DependencyBag bag)
        {
            Debug.Assert(_windUpAnimation.Clip.isLooping, "A throwing windup animation must be looping!");
            bag.ForceGet(out _animationHandler);
        }

        public void Begin()
        {
            _windUpTimer = _windUpTime;
            _animationHandler.BeginPlayActionAnimation(this);
            _animationHandler.PlayActionAnimation(this, _leadAnimation, () =>
            {
                _animationHandler.PlayActionAnimation(this, _windUpAnimation);
            });
        }

        public void Finish()
        {
            _animationHandler.EndPlayActionAnimation(this);
        }

        public void Execute(float deltaTime)
        {
            _translationFrame.Value = default;
            
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