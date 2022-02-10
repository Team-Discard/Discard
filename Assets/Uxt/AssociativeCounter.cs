using System;
using System.Collections.Generic;

namespace Uxt
{
    public class AssociativeCounter<TKey>
    {
        private readonly Dictionary<TKey, int> _dict = new Dictionary<TKey, int>();

        public void IncrementKey(TKey key)
        {
            if (_dict.ContainsKey(key))
            {
                ++_dict[key];
            }
            else
            {
                _dict[key] = 1;
            }
        }

        public bool DecrementKey(TKey key)
        {
            if (!_dict.TryGetValue(key, out var count))
            {
                throw new Exception("Could not decrement a key that doesn't exist in the counter");
            }

            if (--_dict[key] == 0)
            {
                _dict.Remove(key);
                return true;
            }

            return false;
        }

        public int CountKey(TKey key)
        {
            if (_dict.TryGetValue(key, out var count))
            {
                return count;
            }

            return 0;
        }
    }
}