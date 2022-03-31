using Annotations;
using UnityEngine;

[Feature(FeatureTag.CameraController)]
[Feature(FeatureTag.PlayerInputHandler)]
public class ThirdPersonOrbitalCameraInputHandler : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField] private CameraPivot _pivot;
    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;
    
    private MainPlayerControl _control;

    private void Awake()
    {
        _control = MainInputHandler.Instance.Control;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        var mouseDelta = _control.Standard.CameraRotate.ReadValue<Vector2>();
        var rotationDelta = mouseDelta * _sensitivity;
        _pivot.Rotate(rotationDelta, _minPitch, _maxPitch);
    }
}