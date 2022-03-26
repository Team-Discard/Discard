using EntitySystem;
using UnityEngine;
using Unstable.Entities;
using Unstable.Utils;

namespace CharacterSystem
{
    public interface IPawnComponent : IComponent<IPawnComponent>
    {
        Vector3 CurrentVelocity { get; }
        Vector3 CurrentForward { get; }
        void SetTranslationFrame(Translation translation);
        Rotation GetRotationFrame();
        void SetRotationFrame(Rotation rotation);
        void TickTranslation(float deltaTime);
        void TickRotation(float deltaTime);
    }

    public static class PawnUtilities
    {
        public static float CalculateForwardSpeed(this IPawnComponent pawn)
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