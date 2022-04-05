using UnityEngine;

namespace Unstable.Entities
{
    public struct Rotation
    {
        public float YawVelocity { get; set; }
        public Quaternion Delta { get; set; }

        public static Rotation Identity { get; } = new()
        {
            YawVelocity = 0.0f,
            Delta = Quaternion.identity
        };

        public Quaternion Apply(float deltaTime, Quaternion rotation)
        {
            rotation = Quaternion.Euler(0.0f, YawVelocity * deltaTime, 0.0f) * Delta * rotation;
            Debug.Log(Delta);
            return rotation;
        }

        public static Rotation operator *(in Rotation lhs, in Rotation rhs)
        {
            return new Rotation
            {
                YawVelocity = lhs.YawVelocity + rhs.YawVelocity,
                Delta = lhs.Delta * rhs.Delta
            };
        }

        public static Rotation operator *(float lhs, in Rotation rhs)
        {
            return new Rotation
            {
                YawVelocity = lhs * rhs.YawVelocity,
                Delta = Quaternion.Slerp(Quaternion.identity, rhs.Delta, lhs)
            };
        }
        
        public override string ToString()
        {
            return $"[Delta: {Delta}, Yaw Velocity: {YawVelocity}]";
        }
    }
}