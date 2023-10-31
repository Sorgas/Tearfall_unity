using System.Collections.Generic;

namespace util.lang {
// stores lists of values by keys. When last value is removed, the list and key is removed.
    public class MultiValueDictionary<K, V> : Dictionary<K, List<V>> {
        public MultiValueDictionary() { }

        public MultiValueDictionary(MultiValueDictionary<K, V> source) {
            foreach (KeyValuePair<K, List<V>> pair in source) {
                Add(pair.Key, pair.Value);
            }
        }

        public void add(K key, V value) {
            if (!ContainsKey(key)) Add(key, new List<V>());
            this[key].Add(value);
        }

        public void addRange(K key, ICollection<V> values) {
            if (!ContainsKey(key)) Add(key, new List<V>());
            this[key].AddRange(values);
        }
        
        public void remove(K key, V value) {
            this[key].Remove(value);
            if (this[key].Count == 0) Remove(key);
        }
 
        public List<V> get(K key) {
            if (!ContainsKey(key)) return new List<V>();
            return this[key];
        }

        public bool contains(K key, V value) {
            return ContainsKey(key) && this[key].Contains(value);
        }

        public MultiValueDictionary<K, V> clone() {
            return new(this);
        }
    }
}