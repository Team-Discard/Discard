using CharacterSystem;
using EntitySystem;
using UnityEngine;

namespace Unstable.Entities
{
    public class CharacterControllerPawn :
        StandardComponent,
        IPawnComponent
    {
        private readonly CharacterController _controller;
        private Translation _translation;
        private Rotation _rotation;
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

        public void SetTranslationFrame(Translation translation) =>
            _translation.UpdateAndAccumulate(translation);

        public Rotation GetRotationFrame() =>
            _rotation;

        public void SetRotationFrame(Rotation rotation) =>
            _rotation = rotation;

        public void TickTranslation(float deltaTime)
        {
            var finalVelocity = _translation.CombineVelocity(
                deltaTime,
                ref _horizontalVelocity);
            _controller.Move(_translation.Displacement);
            _controller.Move(finalVelocity * deltaTime);
            _velocity = _controller.velocity;
        }

        public void TickRotation(float deltaTime)
        {
            var transform = _controller.transform;
            transform.rotation = _rotation.Apply(deltaTime, transform.rotation);
        }
    }
}