﻿using System.Linq;
using ActionSystem;
using CharacterSystem;
using EntitySystem;
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
            var rotationFrame = _pawn.GetRotationFrame().PrepareNextFrame();
            var translationFrame = new TranslationFrame();

            var controlDirection = _controlCameraTransform.transform.forward.ConvertXz2Xy();
            var hasControl = _actionExecutor.Effects.All(e => e.FreeMovementEnabled);

            if (hasControl)
            {
                _inputHandler.UpdateInput(out var inputDirection);
                LocomotionController.ApplyDirectionalMovement(
                    deltaTime, inputDirection, controlDirection, _maxSpeed,
                    ref translationFrame, ref rotationFrame);
            }
            
            translationFrame.TargetVerticalVelocity -= 5.0f;
            LocomotionController.ApplyActionEffects(deltaTime, _actionExecutor.Effects, ref translationFrame);
            
            _pawn.SetTranslationFrame(translationFrame);
            _pawn.SetRotationFrame(rotationFrame);
        }
    }
}