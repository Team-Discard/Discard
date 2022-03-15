using EntitySystem;
using UnityEngine;

namespace Unstable.Entities
{
    public class CharacterControllerPawn : IPawn
    {
        public IEntity Entity { get; }
        
        private readonly CharacterController _controller;
        private TranslationFrame _translationFrame;
        private RotationFrame _rotationFrame;
        private Vector2 _horizontalVelocity;
        private Vector3 _velocity;

        public CharacterControllerPawn(IEntity entity, CharacterController controller)
        {
            _controller = controller;
            Entity = entity;
        }

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