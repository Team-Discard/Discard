using Annotations;
using UnityEngine;

namespace CameraSystem
{
    [Feature(FeatureTag.CameraController)]
    [ExecuteAlways]
    public class LockOnCameraPivot : MonoBehaviour
    {
        [SerializeField] private Transform _pivotTransform;
        [SerializeField] private Transform _targetTransform;

        private void LateUpdate()
        {
            if (_targetTransform == null || _pivotTransform == null) return;
            var targetPos = _targetTransform.position;
            var destRotation = Quaternion.LookRotation(targetPos - _pivotTransform.position);
            var currentRotation = _pivotTransform.rotation;
            currentRotation = destRotation;
            // currentRotation = Quaternion.Slerp(currentRotation, destRotation, 15f * Time.deltaTime);
            // var euler = currentRotation.eulerAngles;
            // euler.x = Mathf.Clamp(euler.x, -_maxPitch, -_minPitch);
            // currentRotation = Quaternion.Euler(euler);
            _pivotTransform.rotation = currentRotation;
        }
    }
}