using System;
using System.Collections.Generic;
using UnityEngine;
using Unstable.Utils;

namespace Unstable.Entities
{
    public class PlayerLocomotionController : MonoBehaviour
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

        public void ApplyActionEffects(float deltaTime, IReadOnlyList<ActionEffects> effects,
            ref TranslationFrame translationFrame)
        {
            foreach (var effect in effects)
            {
                ExtractTranslation(effect, ref translationFrame);
            }

            if (_useGravity)
            {
                UpdateGravity(_fallAndPushConfig, ref translationFrame);
            }
        }

        public void ApplyDirectionalMovement(
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

            rotationFrame.TargetForwardDirection = controlDirection.normalized;
            rotationFrame.Responsiveness = 15.0f;

            var speed = Mathf.InverseLerp(0.0f, 1.0f, inputDirection.magnitude) * maxSpeed;
            var forward = controlDirection.normalized;
            var right = Vector3.Cross(Vector3.up, forward.ConvertXy2Xz()).ConvertXz2Xy();
            var moveDirection = inputDirection.x * right +  inputDirection.y * forward;
            moveDirection.Normalize();
            
            translationFrame.TargetHorizontalVelocity += speed * moveDirection;
            rotationFrame.TargetForwardDirection = moveDirection;
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
                    translationFrame.Displacement += fallAndPush.AbsoluteAmount * Vector3.up;
                    break;
                }
            }
            //
            // if (fallAndPush.Type is FallAndPushType.Fall or FallAndPushType.Push)
            // {
            //     translationFrame.Displacement += fallAndPush.AbsoluteAmount * Vector3.up;
            // }
        }

        private static void ExtractTranslation(ActionEffects effect, ref TranslationFrame translationFrame)
        {
            translationFrame.TargetHorizontalVelocity += effect.HorizontalVelocity;
        }
    }
}