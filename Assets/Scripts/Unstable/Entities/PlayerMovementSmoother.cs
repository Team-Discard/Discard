using System;
using UnityEngine;

namespace Unstable.Entities
{
    public class PlayerMovementSmoother : MonoBehaviour
    {
        [SerializeField] private Transform _rootTransform;
        private Vector3 _effectivePosition;
        private Vector3 _moveToIdealVelocity;
        

        public void Tick(float deltaTime)
        {
            var idealPos = _rootTransform.transform.position;
            _effectivePosition = Vector3.SmoothDamp(_effectivePosition, idealPos, ref _moveToIdealVelocity, 0.05f,
                float.MaxValue, deltaTime);
            transform.position = _effectivePosition;
        }
    }
}