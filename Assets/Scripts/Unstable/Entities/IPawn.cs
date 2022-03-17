using EntitySystem;
using UnityEngine;
using Unstable.Utils;

namespace Unstable.Entities
{
    public interface IPawn : IComponent<IPawn>
    {
        Vector3 CurrentVelocity { get; }
        Vector3 CurrentForward { get; }
        void SetTranslationFrame(TranslationFrame translationFrame);
        RotationFrame GetRotationFrame();
        void SetRotationFrame(RotationFrame rotationFrame);
        void TickTranslation(float deltaTime);
        void TickRotation(float deltaTime);
    }

    public static class PawnUtilities
    {
        public static float CalculateForwardSpeed(this IPawn pawn)
        {
            var velocity2D = pawn.CurrentVelocity.ConvertXz2Xy();
            var direction2D = pawn.CurrentForward.ConvertXz2Xy();
            if (direction2D.magnitude < 0)
            {
                direction2D = Vector2.up;
            }

            direction2D.Normalize();
            var speed = Vector2.Dot(velocity2D, direction2D);
            speed = Mathf.Max(0.0f, speed);

            return speed;
        }
    }
}