using System.Collections.Generic;

namespace util.lang {
    public class MultiValueDictionary<K, V> : Dictionary<K, List<V>> {
        
        public void add(K key, V value) {
            if(!ContainsKey(key)) Add(key, new List<V>());
            this[key].Add(value);
        }

        public void remove(K key, V value) {
            this[key].Remove(value);
            if (this[key].Count == 0) Remove(key);
        }
    }
}