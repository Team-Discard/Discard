using System;
using Annotations;
using UnityEngine;
using Uxt.PropertyDrawers;

namespace CameraSystem
{
    [Feature(FeatureTag.CameraController)]
    [ExecuteAlways]
    public class LockOnCameraPivot : MonoBehaviour
    {
        [SerializeField, EditInPrefabOnly] private Transform _pivotTransform;
        [SerializeField, EditInPrefabOnly] private Transform _targetTransform;
        
        public Transform TargetTransform
        {
            get => _targetTransform;
            set
            {
                _targetTransform = value;
                UpdateRotation();
            }
        }

        public Transform PivotTransform => _pivotTransform;

        private void Update()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            if (_pivotTransform == null) return;
            if (_targetTransform != null)
            {
                var targetPos = _targetTransform.position;
                var destRotation = Quaternion.LookRotation(targetPos - _pivotTransform.position);
                _pivotTransform.rotation = destRotation;
            }
        }
    }
}