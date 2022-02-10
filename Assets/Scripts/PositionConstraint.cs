using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PositionConstraint : MonoBehaviour
    {
        [SerializeField] private Transform _followTransform;

        private void Update()
        {
            transform.position = _followTransform.position;
        }
    }
}