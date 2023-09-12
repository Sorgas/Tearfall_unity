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

        public static Dictionary<K, V> ToDictionary<K, V, T>(this IEnumerable<T> source, Func<T, K> keyMapper, Func<T, V> valueMapper, Func<V, V, V> valueCombiner) {
            Dictionary<K, V> dictionary = new();
            foreach(var element in source) {
                K key = keyMapper(element);
                V value = valueMapper(element);
                if(dictionary.ContainsKey(key)) value = valueCombiner(dictionary[key], value);
                dictionary[key] = value;
            }
            return dictionary;
        }

        public static Dictionary<K, List<V>> toDictionaryOfLists<K, V, T>(this IEnumerable<T> source, Func<T, K> keyMapper, Func<T, V> valueMapper) {
            Dictionary<K, List<V>> dictionary = new();
            foreach (T element in source) {
                K key = keyMapper(element);
                V value = valueMapper(element);
                if(!dictionary.ContainsKey(key)) dictionary.Add(key, new List<V>());
                dictionary[key].Add(value);
            }
            return dictionary;
        }

        public static Dictionary<K, List<T>> toDictionaryOfLists<K, T>(this IEnumerable<T> source, Func<T, K> keyMapper) {
            Dictionary<K, List<T>> dictionary = new();
            foreach (T element in source) {
                K key = keyMapper(element);
                if(!dictionary.ContainsKey(key)) dictionary.Add(key, new List<T>());
                dictionary[key].Add(element);
            }
            return dictionary;
        }
    }
}
