using Annotations;
using CameraSystem;
using UnityEngine;
using UnityEngine.InputSystem;

[Feature(FeatureTag.CameraController)]
[Feature(FeatureTag.PlayerInputHandler)]
public class ThirdPersonOrbitalCameraInputHandler : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField] private FreeOrbitalCameraPivot _pivot;
    [SerializeField] private float _controllerSpeedMultiplier;
    private MainPlayerControl _control;
    

    private void Awake()
    {
        _control = MainInputHandler.Instance.Control;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        var mouseDelta = _control.Standard.CameraRotate.ReadValue<Vector2>();
        var multipler = 1.0f;
        if (Gamepad.current != null)
        {
            multipler = Time.deltaTime;
        }
        var rotationDelta = mouseDelta * _sensitivity * multipler * _controllerSpeedMultiplier;
        _pivot.Rotate(rotationDelta);
    }
}