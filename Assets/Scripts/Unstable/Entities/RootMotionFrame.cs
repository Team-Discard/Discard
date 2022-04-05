﻿using UnityEngine;
using Uxt.InterModuleCommunication;

namespace Unstable.Entities
{
    public class RootMotionFrame
    {
        private readonly FrameData<Vector3> _deltaPosition = new();
        private readonly FrameData<Quaternion> _deltaRotation = new();

        public bool Destroyed { get; private set; } = false;

        public RootMotionFrame()
        {
            _deltaRotation.SetValue(Quaternion.identity);
            _deltaPosition.SetValue(Vector3.zero);
        }

        public void AddRootMotion(Vector3 deltaPosition, Quaternion deltaRotation)
        {
            _deltaPosition.Value += deltaPosition;
            _deltaRotation.Value *= deltaRotation;
        }

        public void Destroy()
        {
            Destroyed = true;
        }

        public Vector3 ConsumeDisplacement()
        {
            var temp = _deltaPosition.Value;
            _deltaPosition.Value = Vector3.zero;
            return temp;
        }

        public Quaternion ConsumeRotation()
        {
            var temp = _deltaRotation.Value;
            _deltaRotation.Value = Quaternion.identity;
            return temp;
        }
    }
}