using System;
using UnityEngine;

namespace Unstable.Entities
{
    [Serializable]
    public class FallAndPushConfig
    {
        public CapsuleCollider capsule;
        public Transform feetTransform;

        public Vector3 GetOrigin() => capsule.GetBottomPoint();
        public float GetLength() => capsule.GetBottomPoint().y - feetTransform.position.y;

    }
}