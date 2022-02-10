using System.Collections.Generic;
using UnityEngine;

namespace Unstable
{
    public struct ActionEffects
    {
        public Vector2 HorizontalVelocity { get; set; }
        public bool MovementSpeedModification { get; set; }
        public bool MovementEnabled { get; set; }
        public IEnumerable<IDamageVolume> DamageVolumes { get; set; }
    }
}