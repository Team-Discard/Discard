using System;
using System.Collections.Generic;
using Animancer;
using CharacterSystem;
using EntitySystem;
using JetBrains.Annotations;
using UnityEngine;
using WeaponSystem;
using Object = UnityEngine.Object;

namespace Unstable.Entities
{
    public class PawnAnimationHandler :
        StandardComponent,
        IComponent<PawnAnimationHandler>,
        ITicker
    {
        private const string SystemName = "Pawn Animation Handler";

        private readonly IPawnComponent _pawn;
        private readonly AnimancerComponent _animancer;
        private readonly List<IAction> _activeActions = new();
        private bool _hasActionAnimations;
        private bool _locomotionAnimationDirty = true;

        private StandardWeaponLocomotionAnimationSet _locomotionAnimationSet;

        private LinearMixerState _locomotionMixerState;
        private float _absoluteLocomotionSpeed;

        private int _freeAnimationCount = 0;

        public PawnAnimationHandler(
            IPawnComponent pawn,
            AnimancerComponent animancer,
            [NotNull] StandardWeaponLocomotionAnimationSet startingLocomotionAnimationSet)
        {
            _pawn = pawn;
            _animancer = animancer;
            _locomotionAnimationSet = startingLocomotionAnimationSet;
        }

        public void SetLocomotionAnimations([NotNull] StandardWeaponLocomotionAnimationSet animationSet)
        {
            if (animationSet != _locomotionAnimationSet)
            {
                _locomotionAnimationDirty = true;
            }

            _locomotionAnimationSet = animationSet;
        }

        public void SetAbsoluteSpeed(float forwardSpeed)
        {
            _absoluteLocomotionSpeed = forwardSpeed;
        }

        /// <summary>
        /// Tells the pawn animation handler that it is currently playing
        /// the animation for <paramref name="action"/>. This prevents
        /// the animation handler from reverting back to the locomotion animation
        /// when there is a small gap between two animations played by an action.
        /// </summary>
        /// <param name="action"></param>
        public void BeginPlayActionAnimation(IAction action)
        {
            if (_activeActions.Contains(action))
            {
                Debug.LogError($"[{SystemName}] The animation for {action} is already playing", action as Object);
                return;
            }

            _activeActions.Add(action);

            Debug.Log($"[{SystemName}] Begin playing animation for {action}", _animancer);
        }

        /// <summary>
        /// Tells the pawn animation handler that it is no longer playing
        /// the animation for <paramref name="action"/>. See <see cref="BeginPlayActionAnimation"/>
        /// for more details.
        /// </summary>
        /// <param name="action"></param>
        public void EndPlayActionAnimation(IAction action)
        {
            if (!_activeActions.Remove(action))
            {
                Debug.LogError($"[{SystemName}] The animation for {action} has been stopped or hasn't start yet",
                    action as Object);
            }

            Debug.Log($"[{SystemName}] Stop playing animation for {action}", _animancer);
        }

        public void PlayActionAnimation(IAction action, ITransition transition, Action doneCallback)
        {
            if (!_activeActions.Contains(action))
            {
                Debug.LogError($"The action '{action}' is not currently active on the animation handler. " +
                               $"You might want to call {nameof(BeginPlayActionAnimation)} first.",
                    _animancer.gameObject);
                return;
            }

            var state = _animancer.Play(transition);
            state.Events.OnEnd = () =>
            {
                state.Events.OnEnd = null;
                doneCallback?.Invoke();
            };
        }

        public void PlayAnimation(ITransition transition, Action doneCallback)
        {
            ++_freeAnimationCount;
            var state = _animancer.Play(transition);
            state.Events.OnEnd = () =>
            {
                --_freeAnimationCount;
                state.Events.OnEnd = null;
                doneCallback?.Invoke();
            };
        }

        public void Tick(float deltaTime)
        {
            SetAbsoluteSpeed(_pawn.CalculateForwardSpeed());
            
            var hasNonLocomotionAnimation = _activeActions.Count > 0 || _freeAnimationCount > 0;

            if (!_hasActionAnimations && hasNonLocomotionAnimation)
            {
                // nothing is done here
            }
            else if (_hasActionAnimations && !hasNonLocomotionAnimation || _locomotionAnimationDirty)
            {
                _locomotionMixerState =
                    _animancer.Play(_locomotionAnimationSet.Transition) as LinearMixerState;
            }

            _locomotionAnimationDirty = false;
            _hasActionAnimations = hasNonLocomotionAnimation;
            
            if (!_hasActionAnimations)
            {
                SyncLocomotionAnimationSpeed(deltaTime);
            }
        }

        private void SyncLocomotionAnimationSpeed(float deltaTime)
        {
            if (_locomotionMixerState == null) return;

            _locomotionMixerState.Parameter = Mathf.Lerp(_locomotionMixerState.Parameter,
                _absoluteLocomotionSpeed, 15.0f * deltaTime);

            var children = _locomotionMixerState.ChildStates;
            var currentAnimationVelocity = 0.0f;
            if (_absoluteLocomotionSpeed < 0.01f) return;

            // loops starts with 1 because the 0-th index is the idle animation
            for (var i = 1; i < children.Count; ++i)
            {
                currentAnimationVelocity +=
                    children[i].Clip.averageSpeed.z * children[i].EffectiveWeight;
            }

            if (currentAnimationVelocity < 0.01f) return;

            var multiplier = _absoluteLocomotionSpeed / currentAnimationVelocity;

            if (Mathf.Approximately(multiplier, 1.0f)) return;

            for (var i = 1; i < children.Count; ++i)
            {
                children[i].Speed = multiplier;
            }
        }
    }
}