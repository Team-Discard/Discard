using CharacterSystem;
using EntitySystem;
using UnityEngine;

namespace Unstable.Entities
{
    public class CharacterControllerPawn :
        StandardComponent,
        IPawn
    {
        private readonly CharacterController _controller;
        private TranslationFrame _translationFrame;
        private RotationFrame _rotationFrame;
        private Vector2 _horizontalVelocity;
        private Vector3 _velocity;
        private bool _enabledInternal;

        public CharacterControllerPawn(CharacterController controller)
        {
            _controller = controller;
        }

        bool IComponent.EnabledInternal => true;
        public Vector3 CurrentVelocity => _velocity;
        public Vector3 CurrentForward => _controller.transform.forward;

        public void SetTranslationFrame(TranslationFrame translationFrame) =>
            _translationFrame.UpdateAndAccumulate(translationFrame);

        public RotationFrame GetRotationFrame() =>
            _rotationFrame;

        public void SetRotationFrame(RotationFrame rotationFrame) =>
            _rotationFrame = rotationFrame;

        public void TickTranslation(float deltaTime)
        {
            var finalVelocity = _translationFrame.CombineVelocity(
                deltaTime,
                ref _horizontalVelocity);
            _controller.Move(_translationFrame.Displacement);
            _controller.Move(finalVelocity * deltaTime);
            _velocity = _controller.velocity;
        }

        public void TickRotation(float deltaTime)
        {
            var transform = _controller.transform;
            transform.rotation = _rotationFrame.Apply(deltaTime, transform.rotation);
        }
    }
}