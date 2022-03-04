using System;
using JetBrains.Annotations;

namespace Uxt
{
    public class DelayedMethod<TParam, TRet>
    {
        private TParam _value;
        private Action<TRet> _onSuccess;
        private Action _onFail;

        public bool HasValue { get; private set; }

        public bool Consume(out TParam output, out Action<TRet> successCallback)
        {
            if (HasValue)
            {
                output = _value;
                successCallback = _onSuccess;
                HasValue = false;
                _value = default;
                _onSuccess = null;
                _onFail = null;
                return true;
            }
            else
            {
                output = default;
                successCallback = default;
                return false;
            }
        }

        public void Clear()
        {
            if (HasValue)
            {
                _onFail?.Invoke();
                _onFail = null;
                _onSuccess = null;
            }

            _value = default;
            HasValue = false;
        }

        public void InvokeDelayed([NotNull] TParam val, Action<TRet> onSuccess = null, Action onFail = null)
        {
            if (HasValue)
            {
                _onFail?.Invoke();
            }

            _value = val;
            HasValue = true;

            _onSuccess = onSuccess;
            _onFail = onFail;
        }

        public TParam Value =>
            HasValue ? _value : throw new NullReferenceException("The trigger does not contain a value");
    }
}