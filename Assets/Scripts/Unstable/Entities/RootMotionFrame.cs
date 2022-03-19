using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unstable.Entities
{
    public class RootMotionFrame : MonoBehaviour
    {
        private Animator _animator;
        private bool _accumulatesDisplacement;
        private Vector3 _displacement;

        private Dictionary<object, Vector3> _displacements;

        public Vector3 Velocity { get; private set; }

        private void Awake()
        {
            _displacements = new Dictionary<object, Vector3>();
            _animator = GetComponent<Animator>();
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
            }
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
            return ret;
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

        private void OnDestroy()
        {
            if (_displacements.Count > 0)
            {
                Debug.LogError("There are still objects depending on this root motion frame when it is destroyed. " +
                               "Possible bug?");
                var objectNameList = string.Join("\n", _displacements.Keys.Select(k => k.ToString()));
                Debug.LogError($"Affected objects:\n{objectNameList}");
            }
        }
    }
}