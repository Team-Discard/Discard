using UnityEngine;

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
    }
}