using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Uxt;
using Uxt.Debugging;

namespace Unstable.Entities
{
    public class RootMotionFrame : MonoBehaviour
    {
        private Animator _animator;

        private Dictionary<object, Vector3> _displacements;

#if UNITY_ASSERTIONS
        private AssociativeCounter<object> _frameCount;
#endif

        public Vector3 Velocity { get; private set; }

        private Vector3 _temp;
        
        private void Awake()
        {
            _temp = Vector3.zero;
            
            _animator = GetComponent<Animator>();
            _animator.applyRootMotion = true;
            
            _displacements = new Dictionary<object, Vector3>();
            _animator = GetComponent<Animator>();

            _displacementKeyBuffer = new List<object>();

#if UNITY_ASSERTIONS
            _frameCount = new AssociativeCounter<object>();
#endif
        }

        [Obsolete("Use the parameterized version instead")]
        public void BeginAccumulateDisplacement()
        {
            BeginAccumulateDisplacement(this);
        }

        [Obsolete("Use the parameterized version instead")]
        public void EndAccumulateDisplacement()
        {
            EndAccumulateDisplacement(this);
        }

        [Obsolete("Use the parameterized version instead")]
        public Vector3 ConsumeDisplacement()
        {
            return ConsumeDisplacement(this);
        }

        public void BeginAccumulateDisplacement(object key)
        {
            if (_displacements.ContainsKey(key))
            {
                Debug.LogError($"Key '{key}' is already accumulating displacement on this root motion frame.");
                return;
            }

            _displacements[key] = Vector3.zero;
        }

        public void EndAccumulateDisplacement(object key)
        {
            if (!_displacements.Remove(key))
            {
                Debug.LogError($"Key '{key}' is NOT accumulating displacement on this root motion frame.");
                return;
            }
#if UNITY_ASSERTIONS
            _frameCount.SetKey(key, 0);
#endif
        }

        public Vector3 ConsumeDisplacement(object key)
        {
            if (!_displacements.ContainsKey(key))
            {
                Debug.LogError($"Key '{key}' is already accumulating displacement on this root motion frame.");
                return Vector3.zero;
            }

            var ret = _displacements[key];
            _displacements[key] = Vector3.zero;
#if UNITY_ASSERTIONS
            _frameCount.SetKey(key, 0);
#endif
            return ret;
        }

        private List<object> _displacementKeyBuffer;

        private void OnAnimatorMove()
        {
            var deltaPosition = _animator.deltaPosition;
            
            _temp += deltaPosition;
            DebugMessageManager.AddOnScreen($"distance: {_temp}", 77, Color.red, 0.1f);

            var deltaTime = Mathf.Max(0.001f, Time.deltaTime);
            Velocity = deltaPosition / deltaTime;

            _displacements.Keys.ToList(_displacementKeyBuffer);
            
            foreach (var key in _displacementKeyBuffer)
            {
                _displacements[key] += deltaPosition;
#if UNITY_ASSERTIONS
                if (_frameCount.IncrementKey(key) >= 2)
                {
                    Debug.LogError("You have been accumulating displacement on this root motion frame " +
                                   "for more than 2 frames, but haven't consumed it. Possible bug?");
                    _displacements[key] = deltaPosition;
                }
#endif
            }
        }
    }
}