using Annotations;
using UnityEngine;

[Feature(FeatureTag.CameraController)]
public class CameraPivot : MonoBehaviour
{
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
        transform.rotation = rotation;
    }

    public void Rotate(Vector2 delta, float minPitch, float maxPitch)
    {
        _pitch += delta.y;
        _yaw += delta.x;
        
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
        _yaw = Mathf.Repeat(_yaw, 360.0f);
    }
}
