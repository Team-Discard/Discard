using System;
using JetBrains.Annotations;

namespace Uxt
{
    public class Trigger<T>
    {
        private T _value;
        public bool HasValue { get; private set; }

        public bool Consume(out T output)
        {
            if (HasValue)
            {
                output = _value;
                _value = default;
                HasValue = false;
                return true;
            }
            else
            {
                output = default;
                return false;
            }
        }

        public void Clear()
        {
            _value = default;
            HasValue = false;
        }

        public void Set([NotNull] T val)
        {
            _value = val;
            HasValue = true;
        }

        public T Value => HasValue ? _value : throw new NullReferenceException("The trigger does not contain a value");
    }
}