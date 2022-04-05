using System.Collections.Generic;
using UnityEngine;

namespace MotionSystem
{
    public class RootMotionSource : MonoBehaviour
    {
        private Animator _animator;

        private List<RootMotionFrame> _frames;
        public Vector3 Velocity { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.applyRootMotion = true;
            _frames = new List<RootMotionFrame>();
        }

        public RootMotionFrame BeginAccumulate()
        {
            var frame = new RootMotionFrame();
            _frames.Add(frame);
            return frame;
        }

        private void OnAnimatorMove()
        {
            _frames.RemoveAll(f => f.Destroyed);

            var deltaPosition = _animator.deltaPosition;
            var deltaRotation = _animator.deltaRotation;
            var deltaTime = Mathf.Max(0.001f, Time.deltaTime);
            Velocity = deltaPosition / deltaTime;

            foreach (var frame in _frames)
            {
                frame.AddRootMotion(deltaPosition, deltaRotation);
            }
        }
    }
}
