using UnityEngine;

namespace Unstable
{
    public struct ActionEffects
    {
        public Vector2 HorizontalVelocity { get; set; }
        public Vector3 Displacement { get; set; }
        public bool FreeMovementEnabled { get; set; }
        public bool Interruptable { get; set; }
    }
}