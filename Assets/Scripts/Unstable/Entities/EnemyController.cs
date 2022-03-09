﻿using System;
using Animancer;
using UnityEngine;
using Unstable.Utils;
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

            var translationFrame = new TranslationFrame();
            var rotationFrame = new RotationFrame();

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
                    _attackAnimationPlayed = true;
                    _animationHandler.PlayAnimation(
                        _stabAnimation,
                        () =>
                        {
                            _enemyAI.IsSlashing = false;
                            _attackAnimationPlayed = false;
                        });
                }
                translationFrame.TargetHorizontalVelocity += _rootMotionFrame.Velocity.ConvertXz2Xy();
            }


            _locomotionController.ApplyGravity(deltaTime, ref translationFrame);

            _enemyPawn.SetTranslationFrame(translationFrame);
            _enemyPawn.SetRotationFrame(rotationFrame);

            _animationHandler.SetAbsoluteSpeed(_enemyPawn.CalculateForwardSpeed());
            _animationHandler.Tick(deltaTime);
        }
    }
}