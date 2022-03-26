using UnityEngine;
using Unstable.Utils;

namespace Unstable.Entities
{
    public struct MoveTowardsParams
    {
        public Vector3 MyPos { get; set; }
        public Vector3 TargetPos { get; set; }
        public float Speed { get; set; }
    }

    public static class LocomotionControllerExt
    {
        public static void MoveTowards(
            this LocomotionController locomotionController,
            float deltaTime,
            in MoveTowardsParams moveTowardsParams,
            ref Translation translation,
            ref Rotation rotation)
        {
            var inputDirection = Vector2.up;
            var controlDirection = (moveTowardsParams.TargetPos - moveTowardsParams.MyPos).ConvertXz2Xy();
            var maxSpeed = moveTowardsParams.Speed;

            LocomotionController.ApplyDirectionalMovement(
                deltaTime,
                inputDirection,
                controlDirection,
                maxSpeed,
                ref translation,
                ref rotation);
        }
    }
}