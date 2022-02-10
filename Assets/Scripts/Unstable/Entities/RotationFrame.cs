using UnityEngine;

namespace Unstable.Entities
{
    public struct RotationFrame
    {
        public Vector2 TargetForwardDirection { get; set; }
        public float Responsiveness { get; set; }
    }
}