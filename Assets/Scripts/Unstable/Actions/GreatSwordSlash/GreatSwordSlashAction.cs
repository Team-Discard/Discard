using System;
using Animancer;
using UnityEngine;
using Unstable.Entities;
using Unstable.Utils;

namespace Unstable.Actions.GreatSwordSlash
{
    public class GreatSwordSlashAction : MonoBehaviour, IAction
    {
        private PlayerPawn _playerPawn;

        [SerializeField] private ClipTransition _preparationClip;
        [SerializeField] private ClipTransition _executionClip;
        [SerializeField] private ClipTransition _idleClip;

        [SerializeField] private float _preparationTime;
        
        [SerializeField] private AnimancerComponent _animancer;

        private ActionStage _stage;
        private bool _preparationClipDone;
        private float _preparationTimer;
        private bool _executionClipDone;
        private bool _recoveryClipDone;

        private RootMotionFrame _rootMotionFrame;
        
        public void Begin()
        {
            _animancer.Play(_preparationClip).Events.OnEnd = () => { _preparationClipDone = true; };
        }

        public ActionEffects Execute(float deltaTime)
        {
            var effects = new ActionEffects
            {
                FreeMovementEnabled = false
            };
            switch (_stage)
            {
                case ActionStage.Preparation:
                {
                    effects.Interruptable = true;
                    _preparationTimer -= deltaTime;
                    if (_preparationClipDone || _preparationTimer <= 0)
                    {
                        _stage = ActionStage.Execution;
                        var state = _animancer.Play(_executionClip);
                        state.Events.OnEnd = () =>
                        {
                            _executionClipDone = true;
                            state.Events.OnEnd = null;
                        };
                    }
                    break;
                }
                case ActionStage.Execution:
                {
                    effects.Interruptable = false;
                    effects.HorizontalVelocity += _rootMotionFrame.Velocity.ConvertXz2Xy();
                    if (_executionClipDone)
                    {
                        Completed = true;
                        _animancer.Play(_idleClip);
                    }
                    break;
                }
                case ActionStage.Recovery:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return effects;
        }

        public bool Completed { get; private set; }

        public void Init(PlayerPawn pawn)
        {
            _playerPawn = pawn;
            _stage = ActionStage.Preparation;
            _preparationClipDone = false;
            _executionClipDone = false;
            _recoveryClipDone = false;
            _rootMotionFrame = _playerPawn.RootMotionFrame;
            _preparationTimer = _preparationTime;
        }
    }
}