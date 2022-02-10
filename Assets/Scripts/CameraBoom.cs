using System;
using Annotations;
using UnityEngine;

[Feature(FeatureTag.CameraController)]
public class CameraBoom : MonoBehaviour
{
    [SerializeField] private float _maxLength;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _cameraPivot;

    private void Update()
    {
        Debug.Assert(_cameraTransform != null);
        Debug.Assert(_cameraPivot != null);

        var targetPos = _cameraPivot.position - _cameraPivot.forward * _maxLength;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetPos, 0.8f);
        _cameraTransform.LookAt(_cameraPivot.transform.position);
    }
}