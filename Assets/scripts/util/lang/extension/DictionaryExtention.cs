using System;
using System.Collections.Generic;
using System.Linq;

namespace util.lang.extension {
    public static class DictionaryExtention {

        public static void removeByPredicate<K, V>(this Dictionary<K, V> source, Predicate<V> predicate) {
            foreach (K key in source.Keys.Where(key => predicate.Invoke(source[key])).ToList()) {
                source.Remove(key);
            }
        }
    }
}