using System;

namespace Uxt.InterModuleCommunication
{
    public class DelayedMethod<TParam, TRet>
    {
        private TParam _value;
        private Action<TRet> _onSuccess;
        private Action _onFail;

        public bool HasValue { get; private set; }

        public bool Consume(out TParam param, out Action<TRet> successCallback)
        {
            if (HasValue)
            {
                param = _value;
                successCallback = _onSuccess;
                HasValue = false;
                _value = default;
                _onSuccess = null;
                _onFail = null;
                return true;
            }
            else
            {
                param = default;
                successCallback = default;
                return false;
            }
        }

        public bool Consume()
        {
            return Consume(out _, out _);
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

        public void InvokeDelayed(TParam val = default, Action<TRet> onSuccess = null, Action onFail = null)
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