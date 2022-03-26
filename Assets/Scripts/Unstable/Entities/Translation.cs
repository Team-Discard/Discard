using UnityEngine;
using Unstable.Utils;

namespace Unstable.Entities
{
    public struct Translation
    {
        [FrameData(FrameDataType.Sample)] public Vector2 TargetHorizontalVelocity { get; set; }

        [FrameData(FrameDataType.Sample)] public Vector2 ImmediateHorizontalVelocity { get; set; }

        [FrameData(FrameDataType.Sample)] public float TargetVerticalVelocity { get; set; }

        [FrameData(FrameDataType.Sample)] public Vector3 Displacement { get; set; }

        public static Translation operator +(Translation lhs, in Translation rhs)
        {
            lhs.TargetHorizontalVelocity += rhs.TargetHorizontalVelocity;
            lhs.ImmediateHorizontalVelocity += rhs.ImmediateHorizontalVelocity;
            lhs.TargetVerticalVelocity += rhs.TargetVerticalVelocity;
            lhs.Displacement += rhs.Displacement;
            return lhs;
        }
        
        public void UpdateAndAccumulate(Translation other)
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
                velocityResponsiveness * deltaTime);

            finalVelocity += horizontalVelocity.ConvertXy2Xz();
            finalVelocity += ImmediateHorizontalVelocity.ConvertXy2Xz();
            finalVelocity += TargetVerticalVelocity * Vector3.up;

            return finalVelocity;
        }

        public override string ToString()
        {
            return Displacement.ToString();
        }
    }
}