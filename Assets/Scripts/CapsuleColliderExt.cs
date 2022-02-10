using UnityEngine;

public static class CapsuleColliderExt
{
    public static Vector3 GetBottomPoint(this CapsuleCollider collider)
    {
        var tr = collider.transform;
        var center = tr.TransformPoint(collider.center);
        var height = collider.height;
        return center + height * 0.5f * Vector3.down;
    }
}