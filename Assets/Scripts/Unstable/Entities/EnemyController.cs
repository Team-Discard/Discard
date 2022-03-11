using System;
using Animancer;
using UnityEngine;
using Unstable.Utils;
using Uxt.Debugging;
using WeaponSystem;

namespace Unstable.Entities
{
    public class EnemyController :
        MonoBehaviour,
        IEnemy
    {
        [SerializeField] private LocomotionController _locomotionController;
        [SerializeField] private EnemyPawn _enemyPawn;
        [SerializeField] private EnemyAI _enemyAI;
        [SerializeField] private float _speed;
        [SerializeField] private StandardWeaponLocomotionAnimationSet _defaultAnimationSet;
        [SerializeField] private ClipTransition _stabAnimation;
        [SerializeField] private RootMotionFrame _rootMotionFrame;
        [SerializeField] private float _maxAngularVelocityDuringAttack;
        [SerializeField] private float _rotationThreshold;

        private PawnAnimationHandler _animationHandler;

        private bool _attackAnimationPlayed;

        private void Awake()
        {
            _animationHandler = new PawnAnimationHandler(
                _enemyPawn,
                GetComponentInChildren<AnimancerComponent>(),
                _defaultAnimationSet);
            _attackAnimationPlayed = false;
        }

        public void Tick(float deltaTime)
        {
            _enemyAI.Tick(deltaTime);

            DebugMessageManager.AddOnScreen($"Attack playing: {_enemyAI.IsMoving}", 42, Color.blue, 0.0f);
            
            var translationFrame = new TranslationFrame();
            var rotationFrame = _enemyPawn.GetRotationFrame().PrepareNextFrame();

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
                    _animationHandler.PlayAnimation(
                        _stabAnimation,
                        () =>
                        {
                            _enemyAI.IsSlashing = false;
                            _attackAnimationPlayed = false;
                            _rootMotionFrame.EndAccumulateDisplacement();
                        });
                }
                else
                {
                    translationFrame.Displacement += _rootMotionFrame.Displacement;
                    // translationFrame.TargetHorizontalVelocity += _rootMotionFrame.Velocity.ConvertXz2Xy();
                    // Debug.Log($"{_rootMotionFrame.Velocity.ConvertXz2Xy()}");
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


            _locomotionController.ApplyGravity(deltaTime, ref translationFrame);

            _enemyPawn.SetTranslationFrame(translationFrame);
            _enemyPawn.SetRotationFrame(rotationFrame);

            _animationHandler.SetAbsoluteSpeed(_enemyPawn.CalculateForwardSpeed());
            _animationHandler.Tick(deltaTime);
        }
    }
}