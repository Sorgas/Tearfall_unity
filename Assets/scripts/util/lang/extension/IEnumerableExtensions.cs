using System;
using System.Collections.Generic;

namespace util.lang.extension {
    public static class IEnumerableExtensions {
        public static IEnumerable<T> Apply<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var e in source) {
                action(e);
                yield return e;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var e in source) {
                action(e);
            }
        }
        
        public static Dictionary<K, V> ToDictionary<K, V, T>(this IEnumerable<T> source, Func<T, K> keyMapper, Func<T, V> valueMapper, Func<V, V, V> valueCombiner) {
            Dictionary<K, V> dictionary = new Dictionary<K, V>();
            foreach(var element in source) {
                K key = keyMapper(element);
                V value = valueMapper(element);
                if(dictionary.ContainsKey(key)) value = valueCombiner(dictionary[key], value);
                dictionary[key] = value;
            }
            return dictionary;
        }
    }
}
