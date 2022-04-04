using System.Linq;
using ActionSystem;
using CharacterSystem;
using EntitySystem;
using InteractionSystem;
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

        public StandardPlayerController(IPawnComponent pawn, Transform controlCameraTransform, IActionExecutorComponent actionExecutor,
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
            if (InteractionEventSystem.IsInteracting) return;
            
            var rotationFrame = _pawn.GetRotationFrame().PrepareNextFrame();
            var translationFrame = new Translation();

            var controlDirection = _controlCameraTransform.transform.forward.ConvertXz2Xy();
            var hasControl = !_actionExecutor.TranslationFrame.HasValue && !_actionExecutor.RotationFrame.HasValue;

            if (hasControl)
            {
                _inputHandler.UpdateInput(out var inputDirection);
                LocomotionController.ApplyDirectionalMovement(
                    deltaTime, inputDirection, controlDirection, _maxSpeed,
                    ref translationFrame, ref rotationFrame);
            }
            
            translationFrame.TargetVerticalVelocity -= 5.0f;
            LocomotionController.ApplyActionEffects(deltaTime, _actionExecutor, ref translationFrame);
            
            _pawn.SetTranslationFrame(translationFrame);
            _pawn.SetRotationFrame(rotationFrame);
        }
    }
}