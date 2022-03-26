using System;
using UnityEngine;

namespace Uxt
{
    /// <summary>
    /// Represents data that is only valid for a specific number of frames (default is 1)
    /// </summary>
    public class FrameData<T> : IReadOnlyFrameData<T> where T : struct
    {
        private readonly int _validFrameCount;
        private int _lastUpdatedFrame = -9999;
        private T _val;

        public static IReadOnlyFrameData<T> NoValue { get; } = new FrameData<T>();

        public FrameData(int validFrameCount = 1)
        {
            _validFrameCount = validFrameCount;
        }

        public void SetValue(T val)
        {
            _lastUpdatedFrame = Time.frameCount;
            _val = val;
        }

        public bool TryReadValue(out T val)
        {
            if (HasValue)
            {
                val = _val;
                return true;
            }

            val = default;
            return false;
        }

        public bool HasValue => Time.frameCount - _lastUpdatedFrame <= _validFrameCount;

        public T ForceReadValue()
        {
            SetValue(default);
            return _val;
        }

        public bool Add(IReadOnlyFrameData<T> rhs, Func<T, T, T> op)
        {
            if (!rhs.TryReadValue(out var rhsVal)) return false;
            SetValue(op(ForceReadValue(), rhsVal));
            return true;
        }

        public override string ToString()
        {
            return HasValue ? _val.ToString() : "<no value>";
        }
    }
}