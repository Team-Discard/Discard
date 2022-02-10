using UnityEngine;

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
    }
}