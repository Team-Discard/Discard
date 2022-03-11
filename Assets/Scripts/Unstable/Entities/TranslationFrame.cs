using UnityEngine;
using Unstable.Utils;

namespace Unstable.Entities
{
    public struct TranslationFrame
    {
        [FrameData(FrameDataType.Sample)] public Vector2 TargetHorizontalVelocity { get; set; }

        [FrameData(FrameDataType.Sample)] public Vector2 ImmediateHorizontalVelocity { get; set; }

        [FrameData(FrameDataType.Sample)] public float TargetVerticalVelocity { get; set; }

        [FrameData(FrameDataType.Sample)] public Vector3 Displacement { get; set; }

        public void UpdateAndAccumulate(TranslationFrame other)
        {
            TargetHorizontalVelocity = other.TargetHorizontalVelocity;
            ImmediateHorizontalVelocity = other.ImmediateHorizontalVelocity;
            TargetVerticalVelocity = other.TargetVerticalVelocity;
            Displacement = other.Displacement;
        }

        public Vector3 CombineVelocity(
            float deltaTime, 
            ref Vector2 horizontalVelocity,
            float velocityResponsiveness = 15.0f)
        {
            var finalVelocity = Vector3.zero;
            horizontalVelocity = Vector2.Lerp(horizontalVelocity, TargetHorizontalVelocity,
                15f * deltaTime);

            finalVelocity += horizontalVelocity.ConvertXy2Xz();
            finalVelocity += ImmediateHorizontalVelocity.ConvertXy2Xz();
            finalVelocity += TargetVerticalVelocity * Vector3.up;

            return finalVelocity;
        }
    }
}