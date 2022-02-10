using System;
using UnityEngine;

[Obsolete]
public class LegacyOrbitCameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;
    private MainPlayerControl _control;
    private Vector2 _previousMousePosition;
    private float _pitch;
    private float _yaw;

    private void Reset()
    {
        _mouseSensitivity = 0.1f;
    }

    private void Awake()
    {
        _pitch = 0;
        _yaw = 0;
        _control = MainInputHandler.Instance.Control;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _previousMousePosition = _control.Standard.CameraRotate.ReadValue<Vector2>();
    }
    
    private void Update()
    {
        // var currentMousePos = _control.Standard.CameraRotate.ReadValue<Vector2>();
        // var mouseDelta = currentMousePos - _previousMousePosition;
        var mouseDelta = _control.Standard.CameraRotate.ReadValue<Vector2>();
        RotateCamera(mouseDelta * _mouseSensitivity);
        // _previousMousePosition = currentMousePos;
    }

    private void RotateCamera(Vector2 delta)
    {
        _pitch += delta.y;
        _yaw += delta.x;

        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);
        _yaw = Mathf.Repeat(_yaw, 360.0f);

        var rotation = Quaternion.Euler(-_pitch, _yaw, 0.0f);
        _cameraPivot.rotation = rotation;
    }
}