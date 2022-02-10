using System.Collections.Generic;
using UnityEngine;

namespace Unstable.Utils
{
    public static class ObjectPool<T> where T : class, IEnablePooling, new()
    {
        private const int InitialCapacity = 1024;
        private static int _capcity = InitialCapacity;
        private static readonly List<T> _pool = new(InitialCapacity);

        static ObjectPool()
        {
            Debug.Assert(typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)),
                "ObjectPool may not store an object managed by Unity. Use UnityObjectPool instead.");
        }

        public static void SetCapacity(int capacity)
        {
            Debug.Assert(capacity > 0);
            _capcity = capacity;
            if (_pool.Count > capacity)
            {
                _pool.RemoveRange(capacity, _pool.Count - capacity);
            }

            _pool.Capacity = capacity;
        }

        public static T Get()
        {
            if (_pool.Count == 0)
            {
                var ret = new T();
                ret.ResetForPooling();
                return ret;
            }
            else
            {
                var ret = _pool[^1];
                _pool.RemoveAt(_pool.Count - 1);
                return ret;
            }
        }

        public static void Return(ref T item)
        {
            if (_pool.Count >= _capcity)
            {
                Debug.LogWarning($"The object pool of {typeof(T)} has reached maximum capacity ({_capcity})" +
                                 $"which will lead to allocation. Consider resizing the pool");
                item = null;
                return;
            }

            item.ResetForPooling();
            _pool.Add(item);
            item = null;
        }
    }
}