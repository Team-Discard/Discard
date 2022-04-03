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

        [SerializeField, EditInPrefabOnly, Tooltip("The rotation to fallback to if no target is present")]
        private Transform _fallbackRotation;

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
            else
            {
                if (_fallbackRotation != null)
                {
                    _pivotTransform.rotation = _fallbackRotation.rotation;
                }
            }
        }
    }
}