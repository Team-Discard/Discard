using System;
using Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraSystem
{
    [Feature(FeatureTag.CameraController)]
    [ExecuteAlways]
    public class CameraBoom : MonoBehaviour
    {
        [SerializeField] private float _maxLength;

        [FormerlySerializedAs("_cameraTransform")] [SerializeField]
        private Transform _boomEndTransform;

        [SerializeField] private Transform _cameraPivot;
        [SerializeField] private bool _useSpring;
        [SerializeField] private float _cameraRadius;
        [SerializeField] private LayerMask _cameraCollisionLayers;
        private float _targetEffectiveLength;
        private float _effectiveLength;

        private Vector3 _effectivePivotPos;
        private Vector3 _moveToIdealPivotVelocity;

        private void Awake()
        {
            _effectivePivotPos = _cameraPivot.transform.position;
            _moveToIdealPivotVelocity = Vector3.zero;
            _targetEffectiveLength = _effectiveLength = _maxLength;
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                Debug.Assert(_boomEndTransform != null);
                Debug.Assert(_cameraPivot != null);
            }
            else if (_boomEndTransform == null || _cameraPivot == null)
            {
                return;
            }

            _effectiveLength = Mathf.Lerp(_effectiveLength, _targetEffectiveLength, 15.0f * Time.deltaTime);
            
            if (_useSpring)
            {
                _effectivePivotPos =
                    Vector3.SmoothDamp(_effectivePivotPos, _cameraPivot.transform.position,
                        ref _moveToIdealPivotVelocity,
                        0.1f);
            }
            else
            {
                _effectivePivotPos = _cameraPivot.transform.position;
            }

            _boomEndTransform.position = GetTargetCameraPos();
            _boomEndTransform.LookAt(_effectivePivotPos);
        }

        private Vector3 GetTargetCameraPos()
        {
            return _effectivePivotPos - _cameraPivot.transform.forward * _effectiveLength;
        }

        private void FixedUpdate()
        {
            var rayLength = _maxLength + _cameraRadius;
            if (Physics.Raycast(
                    new Ray(_effectivePivotPos,
                        -_cameraPivot.transform.forward),
                    out var hit,
                    rayLength,
                    _cameraCollisionLayers,
                    QueryTriggerInteraction.Ignore))
            {
                _targetEffectiveLength = Mathf.Clamp(hit.distance - _cameraRadius, _cameraRadius, _maxLength);
            }
            else
            {
                _targetEffectiveLength = _maxLength;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_effectivePivotPos, 0.5f);
        }
    }
}