using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Uxt
{
    public class DependencyBag : IEnumerable<object>
    {
        private readonly List<object> _bags = new();

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