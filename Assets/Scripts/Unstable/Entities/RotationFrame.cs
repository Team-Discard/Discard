using UnityEngine;
using Unstable.Utils;

namespace Unstable.Entities
{
    public struct RotationFrame
    {
        public Vector2 TargetForwardDirection { get; set; }
        public float Responsiveness { get; set; }

        public float? OverrideLinearRotation { get; set; }

        public void AddOverrideLinearRotation(float angle)
        {
            OverrideLinearRotation ??= 0.0f;
            OverrideLinearRotation += angle;
        }

        public RotationFrame PrepareNextFrame()
        {
            var ret = this;
            ret.Responsiveness = 15.0f;
            ret.OverrideLinearRotation = null;
            return ret;
        }

        public Quaternion Apply(float deltaTime, Quaternion rotation)
        {
            if (OverrideLinearRotation is { } linearRotation)
            {
                rotation *= Quaternion.Euler(0.0f, linearRotation, 0.0f);
            }
            else
            {
                var targetForward = TargetForwardDirection.ConvertXy2Xz();
                var targetRotation = Quaternion.LookRotation(targetForward);
                var currentRotation = rotation;
                rotation = Quaternion.Slerp(currentRotation, targetRotation, deltaTime * Responsiveness);
            }

            return rotation;
        }
    }
}