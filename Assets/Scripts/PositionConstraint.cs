using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class PositionConstraint : MonoBehaviour
{
    [FormerlySerializedAs("_followTransform")] [SerializeField] private Transform _followTarget;
    [SerializeField] private Transform _follower;

    private void Awake()
    {
        if (_follower == null)
        {
            _follower = transform;
        }
    }

    private void Update()
    {
        if (_follower != null && _followTarget != null)
        {
            _follower.position = _followTarget.position;
        }
    }
}