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
            _rotation.TargetForwardDirection = Vector2.up;
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

            // First move by displacement. This is not factored into velocity
            // calculation because this can be supplied from animation but animation
            // speed is also determined by velocity
            // to:billy todo: make a separate field in Translation to account for displacement not affecting effective final velocity.
            _controller.Move(_translation.Displacement);
            var transform = _controller.transform;
            var oldPosition = transform.position;

            // Then move by velocity
            // This factors into velocity calculation
            _controller.Move(finalVelocity * deltaTime);

            // ReSharper disable once Unity.InefficientPropertyAccess
            var deltaPos = transform.position - oldPosition;

            _velocity = deltaTime > 1e-5 ? deltaPos / deltaTime : Vector3.zero;
        }

        public void TickRotation(float deltaTime)
        {
            var transform = _controller.transform;
            transform.rotation = _rotation.Apply(deltaTime, transform.rotation);
        }
    }
}