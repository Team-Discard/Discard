using System;
using UnityEngine;

namespace Unstable.Entities
{
    public class RootMotionFrame : MonoBehaviour
    {
        private Animator _animator;
        private bool _accumulatesDisplacement;
        private Vector3 _displacement;
        
        public Vector3 Velocity { get; private set; }

        public Vector3 Displacement => _displacement;

        public void BeginAccumulateDisplacement()
        {
            _accumulatesDisplacement = true;
            _displacement = Vector3.zero;
        }

        public void EndAccumulateDisplacement()
        {
            _accumulatesDisplacement = false;
            _displacement = Vector3.zero;
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public Vector3 ConsumeDisplacement()
        {
            var temp = _displacement;
            _displacement = Vector3.zero;
            return temp;
        }
        
        private void OnAnimatorMove()
        {
            var deltaPosition = _animator.deltaPosition;
            var deltaTime = Mathf.Max(0.001f, Time.deltaTime);
            Velocity = deltaPosition / deltaTime;
            if (_accumulatesDisplacement)
            {
                _displacement += deltaPosition;
            }
        }
    }
}