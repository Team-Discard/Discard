using System;
using Annotations;
using UnityEngine;

[Feature(FeatureTag.CameraController)]
public class CameraBoom : MonoBehaviour
{
    [SerializeField] private float _maxLength;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _cameraPivot;

    [SerializeField] private bool _useSpring;
    private Vector3 _effectivePivotPos;
    private Vector3 _moveToIdealPivotVelocity;

    private void Awake()
    {
        _effectivePivotPos = _cameraPivot.transform.position;
        _moveToIdealPivotVelocity = Vector3.zero;
    }

    private void Update()
    {
        Debug.Assert(_cameraTransform != null);
        Debug.Assert(_cameraPivot != null);

        if (_useSpring)
        {
            _effectivePivotPos =
                Vector3.SmoothDamp(_effectivePivotPos, _cameraPivot.transform.position, ref _moveToIdealPivotVelocity,
                    0.1f);
        }
        else
        {
            _effectivePivotPos = _cameraPivot.transform.position;
        }
        _cameraTransform.position = GetIdealCameraPos();
        _cameraTransform.LookAt(_effectivePivotPos);
    }

    private Vector3 GetIdealCameraPos()
    {
        return _effectivePivotPos - _cameraPivot.transform.forward * _maxLength;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_effectivePivotPos, 0.5f);
    }
}