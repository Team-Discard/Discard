using UnityEngine;

namespace Uxt
{
    public static class IkUtility
    {
        public static RigidTransformData MatchStickWithHandlePoints(Transform handleBottom, Transform handleTop)
        {
            var pos1 = handleBottom.position;
            var pos2 = handleTop.position;
            var up = (pos2 - pos1).normalized;
            var center = Vector3.Lerp(pos1, pos2, 0.5f);
            var right1 = Vector3.ProjectOnPlane(handleBottom.right, up);
            var right2 = Vector3.ProjectOnPlane(handleTop.right, up);
            var angle = Vector3.SignedAngle(right1, right2, up);
            var rightAverage = Quaternion.AngleAxis(angle * 0.5f, right1) * right2;
            rightAverage.Normalize();
            var fwd = Vector3.Cross(rightAverage, up).normalized;
            var rotation = Quaternion.LookRotation(fwd, up);
            return new RigidTransformData(center, rotation);
        }

        public static RigidTransformData MoveParentToMatchChild(Transform parent, Transform child,
            RigidTransformData target)
        {
            var matrix = target.ToMatrix() * child.worldToLocalMatrix * parent.localToWorldMatrix;
            return new RigidTransformData(matrix.GetPosition(), matrix.rotation);
        }
    }
}