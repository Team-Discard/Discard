using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;

namespace Uxt.InterModuleCommunication
{
    public class DependencyBag : IEnumerable<object>
    {
        private readonly List<object> _bags = new();

#if UNITY_ASSERTIONS

        private readonly string _stackTrace;

        public DependencyBag()
        {
            _stackTrace = new StackTrace(true).ToString();
        }

#endif

        public bool Get<T>(out T result)
        {
            Debug.Assert(
                _bags.Count(o => o is T) <= 1,
                $"There are multiple objects of {typeof(T)} in the dependency bag");

            foreach (var item in _bags)
            {
                if (item is T t)
                {
                    result = t;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public T ForceGet<T>()
        {
            if (Get(out T result))
            {
                return result;
            }

#if UNITY_ASSERTIONS
            throw new NullReferenceException(
                $"No item of type '{typeof(T)}' inside the dependency bag. Created at:\n{_stackTrace}");
#else
            throw new NullReferenceException($"No item of type '{typeof(T)}' inside the dependency bag.");
#endif
        }

        public void ForceGet<T>(out T output)
        {
            output = ForceGet<T>();
        }

        public void Add(object item)
        {
            _bags.Add(item);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _bags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_bags).GetEnumerator();
        }
    }
}