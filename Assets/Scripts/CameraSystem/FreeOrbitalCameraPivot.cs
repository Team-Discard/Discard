using Annotations;
using CutSceneSystem;
using InteractionSystem;
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

        private void Update()
        {
            // very hacky
            if (InteractionEventSystem.PlayerRestraint > 0) return;
            
            var rotation = Quaternion.Euler(-_pitch, _yaw, 0.0f);
            _pivotTransform.rotation = rotation;
        }

        /// <summary>
        /// Matches the rotation to another pivot so that the transition between them can be seamless.
        /// Example use: when the player gets out of lock-on, sync the free look camera with the lock
        /// on camera
        /// </summary>
        /// <param name="otherPivot">The other camera pivot to sync with</param>
        public void SyncWith(Transform otherPivot)
        {
            var forward = otherPivot.forward;
            var forwardXz = Vector3.ProjectOnPlane(forward, Vector3.up);
            _pitch = Vector3.SignedAngle(forwardXz, forward, Vector3.Cross(forwardXz, forward));
            _yaw = Vector3.SignedAngle(Vector3.forward, forwardXz, Vector3.up);
            SanitizePitchAndYaw();
        }

        private void SanitizePitchAndYaw()
        {
            _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);
            _yaw = Mathf.Repeat(_yaw, 360.0f);
        }

        public void Rotate(Vector2 delta)
        {
            _pitch += delta.y;
            _yaw += delta.x;
            SanitizePitchAndYaw();

        }
    }
}
