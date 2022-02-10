using UnityEngine;

namespace Unstable.Entities
{
    public enum FallAndPushType
    {
        None,
        Push,
        Fall
    }

    public struct FallAndPush
    {
        public FallAndPushType Type { get; private set; }
        public float AbsoluteAmount { get; private set; }

        public static FallAndPush None()
        {
            return new FallAndPush();
        }

        public static FallAndPush Push(float absoluteAmount)
        {
            return new FallAndPush
            {
                Type = FallAndPushType.Push,
                AbsoluteAmount = absoluteAmount
            };
        }

        public static FallAndPush Fall(float absoluteAmount)
        {
            return new FallAndPush
            {
                Type = FallAndPushType.Fall,
                AbsoluteAmount = absoluteAmount
            };
        }
        
        public static FallAndPush Calculate(
            Vector3 stepOrigin, float stepLength, LayerMask groundCheckMask, float gravityAmount,
            float threshold = 0.025f)
        {
            Debug.Assert(stepLength > 0.0f);
            Debug.Assert(threshold > 0.0f);
            Debug.Assert(threshold < stepLength);
            
            var actualStepLength = (stepLength + threshold) * 2.0f;

            var hit = Physics.Raycast(new Ray(stepOrigin, Vector3.down), out var hitInfo, actualStepLength, groundCheckMask);

            if (!hit)
            {
                return Fall(gravityAmount);
            }

            var distance = hitInfo.distance;

            if (distance > stepLength + threshold)
            {
                return Fall(gravityAmount);
            }

            if (distance < stepLength - threshold)
            {
                return Push(stepLength - distance);
            }
            
            return None();
        }
    }
}