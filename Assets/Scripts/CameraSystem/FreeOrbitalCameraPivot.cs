using Annotations;
using UnityEngine;

namespace CameraSystem
{
    [Feature(FeatureTag.CameraController)]
    public class FreeOrbitalCameraPivot : MonoBehaviour
    {
        [SerializeField] private Transform _pivotTransform;
        [SerializeField] private float _minPitch;
        [SerializeField] private float _maxPitch;
    
        private float _pitch;
        private float _yaw;

        private void Awake()
        {
            _pitch = 0;
            _yaw = 0;
        }

        private void LateUpdate()
        {
            var rotation = Quaternion.Euler(-_pitch, _yaw, 0.0f);
            _pivotTransform.rotation = rotation;
        }

        // todo: to:billy minPitch and maxPitch should be properties of the pivot not the controller?
        public void Rotate(Vector2 delta)
        {
            _pitch += delta.y;
            _yaw += delta.x;
        
            _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);
            _yaw = Mathf.Repeat(_yaw, 360.0f);
        }
    }
}
