using System;
using System.Reflection;
using UnityEngine;

namespace Uxt.InterModuleCommunication
{
    /// <summary>
    /// Represents data that is only valid for a specific number of frames (default is 1)
    /// </summary>
    public class FrameData<T> : IReadOnlyFrameData<T> where T : struct
    {
        private static readonly T InternalIdentity = GetIdentity();

        private readonly int _validFrameCount;
        private int _lastUpdatedFrame = -9999;
        private T _val;

        public static IReadOnlyFrameData<T> Identity { get; } = new FrameData<T>(GetIdentity(), 1);

        public FrameData(int validFrameCount = 1) : this(InternalIdentity, validFrameCount)
        {
        }

        public FrameData(T val, int validFrameCount = 1)
        {
            _val = val;
            _validFrameCount = validFrameCount;
        }

        /// <summary>
        /// Gets the "identity" value for a type. E.g. int -> 0, vector3 -> 0, quaternion -> (0, 0, 0, 1)
        /// </summary>
        /// <returns></returns>
        private static T GetIdentity()
        {
            if (typeof(Quaternion) == typeof(T))
            {
                return (T)(object)Quaternion.identity;
            }

            if (typeof(Vector3) == typeof(T))
            {
                return (T)(object)Vector3.zero;
            }

            var type = typeof(T);
            var identityProperty =
                type.GetProperty("Identity", BindingFlags.Static | BindingFlags.Public);
            var getter = identityProperty?.GetGetMethod();
            if (getter == null || getter.ReturnType != typeof(T))
            {
                return default;
            }

            return (T)getter.Invoke(null, Array.Empty<object>());
        }

        private bool Valid => Time.frameCount - _lastUpdatedFrame <= _validFrameCount;

        public T Value
        {
            get
            {
                if (!Valid) _val = InternalIdentity;
                return _val;
            }
            set
            {
                _val = value;
                _lastUpdatedFrame = Time.frameCount;
            }
        }

        public override string ToString()
        {
            return _val.ToString();
        }

        public void UpdateValue(Func<T, T> updater)
        {
            Value = updater(Value);
        }
    }
}