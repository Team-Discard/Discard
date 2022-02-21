using System;
using UnityEngine;

namespace Unstable.Entities
{
    public class RootMotionFrame : MonoBehaviour
    {
        private Animator _animator;

        public Vector3 Velocity { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            var deltaPosition = _animator.deltaPosition;
            var deltaTime = Mathf.Max(0.001f, Time.deltaTime);
            Velocity = deltaPosition / deltaTime;
        }
    }
}