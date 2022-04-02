using Annotations;
using UnityEngine;
using UnityEngine.Serialization;

[Feature(FeatureTag.CameraController)]
[ExecuteAlways]
public class CameraBoom : MonoBehaviour
{
    [SerializeField] private float _maxLength;
    [FormerlySerializedAs("_cameraTransform")] [SerializeField] private Transform _boomEndTransform;
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
        if (Application.isPlaying)
        {
            Debug.Assert(_boomEndTransform != null);
            Debug.Assert(_cameraPivot != null);
        }
        else if (_boomEndTransform == null || _cameraPivot == null)
        {
            return;
        }

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

        _boomEndTransform.position = GetIdealCameraPos();
        _boomEndTransform.LookAt(_effectivePivotPos);
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