﻿using System.Collections.Generic;
using UnityEngine;
using Unstable.Utils;

namespace Unstable.Entities
{
    public class LocomotionController : MonoBehaviour
    {
        [SerializeField] private FallAndPushConfig _fallAndPushConfig;
        [SerializeField] private bool _useGravity;

        public bool UseGravity
        {
            get => _useGravity;
            set => _useGravity = value;
        }

        private void Reset()
        {
            _useGravity = true;
        }

        public static void ApplyActionEffects(float deltaTime, IReadOnlyList<ActionEffects> effects,
            ref TranslationFrame translationFrame)
        {
            foreach (var effect in effects)
            {
                ExtractTranslation(effect, ref translationFrame);
            }
        }

        public void ApplyGravity(float deltaTime, ref TranslationFrame translationFrame)
        {
            if (!_useGravity)
            {
                return;
            }
            UpdateGravity(_fallAndPushConfig, ref translationFrame);
        }

        public static void ApplyDirectionalMovement(
            float deltaTime, 
            Vector2 inputDirection,
            Vector2 controlDirection,
            float maxSpeed,
            ref TranslationFrame translationFrame,
            ref RotationFrame rotationFrame)
        {
            if (controlDirection.magnitude < 0.01f)
            {
                controlDirection = Vector2.up;
            }

            if (inputDirection.magnitude < 0.1f)
            {
                return;
            }

            var speed = Mathf.InverseLerp(0.0f, 1.0f, inputDirection.magnitude) * maxSpeed;
            var forward = controlDirection.normalized;
            var right = Vector3.Cross(Vector3.up, forward.ConvertXy2Xz()).ConvertXz2Xy();
            var moveDirection = inputDirection.x * right +  inputDirection.y * forward;
            moveDirection.Normalize();
            
            translationFrame.TargetHorizontalVelocity += speed * moveDirection;
            if (!Mathf.Approximately(moveDirection.sqrMagnitude, 0))
            {
                rotationFrame.TargetForwardDirection = moveDirection;
            }
            rotationFrame.Responsiveness = 15.0f;
        }

        private static void UpdateGravity(FallAndPushConfig config, ref TranslationFrame translationFrame)
        {
            var fallAndPush = FallAndPush.Calculate(
                config.GetOrigin(),
                config.GetLength(),
                ~0,
                -10.0f * Time.deltaTime);

            switch (fallAndPush.Type)
            {
                case FallAndPushType.Fall:
                {
                    translationFrame.TargetVerticalVelocity += -10.0f;
                    break;
                }
                case FallAndPushType.Push:
                {
                    // translationFrame.Displacement += fallAndPush.AbsoluteAmount * Vector3.up;
                    break;
                }
            }
        }

        private static void ExtractTranslation(ActionEffects effect, ref TranslationFrame translationFrame)
        {
            translationFrame.TargetHorizontalVelocity += effect.HorizontalVelocity;
        }
    }
}