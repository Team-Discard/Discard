using System.Collections.Generic;
using UnityEngine;
using Unstable.Utils;

namespace CameraSystem
{
    public static class LockOnTargetManager
    {
        private static List<Transform> _targets;
        private static float _maxFov;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            _targets = new List<Transform>();
            _maxFov = 60.0f;
        }

        public static void SetMaxFov(float maxFov)
        {
            _maxFov = maxFov;
        }

        public static void RegisterTarget(Transform target)
        {
            Debug.Assert(target != null, "target != null");
            Debug.Assert(!_targets.Contains(target), "!_targets.Contains(target)");
            _targets.Add(target);
        }

        public static void UnregisterTarget(Transform target)
        {
            var existed = _targets.Remove(target);
            Debug.Assert(existed, "existed");
        }

        public static Transform FindBestTarget(Vector3 origin, Vector3 viewVector)
        {
            _targets.RemoveAll(t => t == null);

            Transform bestTarget = null;
            var bestAngle = float.MaxValue;
            var viewVector2D = viewVector.ConvertXz2Xy();

            foreach (var target in _targets)
            {
                var origin2Target = (target.position - origin).ConvertXz2Xy();
                var angle = Vector2.Angle(viewVector2D, origin2Target);
                if (angle > _maxFov * 0.5f || angle >= bestAngle) continue;
                bestAngle = angle;
                bestTarget = target;
            }

            return bestTarget;
        }
    }
}