using ActionSystem;
using MotionSystem;
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

        public static void ApplyActionEffects(
            float deltaTime, 
            IActionExecutorComponent executor,
            ref Translation translation,
            ref Rotation rotation)
        {
            // todo: to:billy the motion system needs to propagate through multiple levels and adding features
            // to it is super painful.

            translation += executor.TranslationFrame.Value;
            rotation *= executor.RotationFrame.Value;
        }

        public void ApplyGravity(float deltaTime, ref Translation translation)
        {
            if (!_useGravity)
            {
                return;
            }

            UpdateGravity(_fallAndPushConfig, ref translation);
        }

        // todo: to:billy this function takes a lot of arguments. Can it be simplified?
        public static void ApplyDirectionalMovement(float deltaTime,
            Vector2 inputDirection,
            Vector2 controlDirection,
            Vector2 currentForward,
            float maxSpeed,
            float maxAngularSpeed,
            ref Translation translation,
            ref Rotation rotation)
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
            var moveDirection = inputDirection.x * right + inputDirection.y * forward;
            moveDirection.Normalize();

            translation.TargetHorizontalVelocity += speed * moveDirection;
            
            if (!Mathf.Approximately(moveDirection.sqrMagnitude, 0))
            {
                var angleDiff = -Vector2.SignedAngle(currentForward, moveDirection);
                rotation.Delta *= Quaternion.Slerp(Quaternion.identity, Quaternion.AngleAxis(angleDiff, Vector3.up), 15f * deltaTime);
                // if (Mathf.Abs(angleDiff) >= 1f)
                // {
                //     rotation.YawVelocity += Mathf.Sign(angleDiff) * maxAngularSpeed;
                // }
            }
        }

        private static void UpdateGravity(FallAndPushConfig config, ref Translation translation)
        {
            var fallAndPush = FallAndPush.Calculate(
                config.GetOrigin(),
                config.GetLength(),
                ~0,
                -10.0f * Time.deltaTime);

            translation.TargetVerticalVelocity += -10.0f;
        }
    }
}