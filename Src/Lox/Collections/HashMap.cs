using System;
using System.Collections.Generic;
using System.Text;

namespace Lox.Collections
{
    public class HashMap<K, V> : Dictionary<K, V>
    {
        public V Get(K key)
        {
            if (TryGetValue(key, out V value))
            {
                return value;
            }

            return default;

        }

        public V Put(K key, V value)
        {
            V old_value = default;

            if (ContainsKey(key))
            {
                old_value = this[key];
                this[key] = value;
            }
            else
            {
                Add(key, value);
            }

            return old_value;
        }

    }
}
