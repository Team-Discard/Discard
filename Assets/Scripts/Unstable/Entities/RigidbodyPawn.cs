using UnityEngine;
using Unstable.Utils;

namespace Unstable.Entities
{
    public class RigidbodyPawn : IPawn
    {
        private readonly Rigidbody _rigidbody;

        private Vector2 _horizontalVelocity;
        private TranslationFrame _translationFrame;

        private RotationFrame _rotationFrame;

        public Vector3 CurrentVelocity => _rigidbody.velocity;
        public Vector3 CurrentForward => _rigidbody.transform.forward;

        public RigidbodyPawn(Rigidbody rigidbody)
        {
            _rotationFrame = new RotationFrame
            {
                Responsiveness = 15.0f,
                TargetForwardDirection = Vector2.up
            };
            _rigidbody = rigidbody;
        }

        public void SetTranslationFrame(TranslationFrame translationFrame)
        {
            _translationFrame.UpdateAndAccumulate(translationFrame);
        }

        public RotationFrame GetRotationFrame() => _rotationFrame;

        public void SetRotationFrame(RotationFrame rotationFrame) => _rotationFrame = rotationFrame;

        public void TickPhysics(float deltaTime)
        {
            var finalVelocity = Vector3.zero;
            _horizontalVelocity = Vector2.Lerp(_horizontalVelocity, _translationFrame.TargetHorizontalVelocity,
                15f * deltaTime);
            
            finalVelocity += _horizontalVelocity.ConvertXy2Xz();
            finalVelocity += _translationFrame.ImmediateHorizontalVelocity.ConvertXy2Xz();
            finalVelocity += _translationFrame.TargetVerticalVelocity * Vector3.up;
            
            _rigidbody.velocity = finalVelocity;
            _rigidbody.MovePosition(_rigidbody.position + _translationFrame.Displacement);
        }

        public void TickRotation(float deltaTime)
        {
            var targetForward = _rotationFrame.TargetForwardDirection.ConvertXy2Xz();
            var targetRotation = Quaternion.LookRotation(targetForward);
            var currentRotation = _rigidbody.rotation;
            _rigidbody.rotation = Quaternion.Slerp(currentRotation, targetRotation, deltaTime * _rotationFrame.Responsiveness);
        }
    }
}