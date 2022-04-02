using System;
using UnityEngine;

[ExecuteInEditMode]
public class PositionConstraint : MonoBehaviour
{
    [SerializeField] private Transform _followTransform;
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
        _follower.position = _followTransform.position;
    }
}