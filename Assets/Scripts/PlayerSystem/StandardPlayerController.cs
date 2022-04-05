using ActionSystem;
using CharacterSystem;
using CutSceneSystem;
using EntitySystem;
using InteractionSystem;
using MotionSystem;
using UnityEngine;
using Unstable.Entities;
using Unstable.Utils;

namespace PlayerSystem
{
    public class StandardPlayerController : StandardComponent, IPawnControllerComponent
    {
        private readonly IPawnComponent _pawn;
        private readonly Transform _controlCameraTransform;
        private readonly IActionExecutorComponent _actionExecutor;
        private readonly PlayerInputHandler _inputHandler;
        private float _maxSpeed;

        public StandardPlayerController(IPawnComponent pawn, Transform controlCameraTransform,
            IActionExecutorComponent actionExecutor,
            PlayerInputHandler inputHandler, float maxSpeed)
        {
            _pawn = pawn;
            _controlCameraTransform = controlCameraTransform;
            _actionExecutor = actionExecutor;
            _inputHandler = inputHandler;
            _maxSpeed = maxSpeed;
        }

        public void Tick(float deltaTime)
        {
            // very hacky
            if (InteractionEventSystem.PlayerRestraint > 0) return;

            var rotationFrame = Rotation.Identity;
            var translationFrame = Translation.Identity;

            var controlDirection = _controlCameraTransform.transform.forward.ConvertXz2Xy();
            var hasControl = _actionExecutor.PlayerControlFactor > 0.01f;

            if (hasControl)
            {
                _inputHandler.UpdateInput(out var inputDirection);
                LocomotionController.ApplyDirectionalMovement(
                    deltaTime, inputDirection, controlDirection,
                    _pawn.CurrentForward.ConvertXz2Xy(),
                    _maxSpeed, 720.0f,
                    ref translationFrame, ref rotationFrame);
            }

            translationFrame.TargetVerticalVelocity -= 5.0f;
            LocomotionController.ApplyActionEffects(deltaTime, _actionExecutor, ref translationFrame,
                ref rotationFrame);

            _pawn.SetTranslation(translationFrame);
            _pawn.SetRotation(rotationFrame);
        }
    }
}