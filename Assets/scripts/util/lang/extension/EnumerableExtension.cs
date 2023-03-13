using System;
using System.Collections.Generic;
using System.Linq;

namespace util.lang.extension {
    public static class EnumerableExtension {

        public static bool includesAll<T>(this IEnumerable<T> current, IEnumerable<T> target) {
            return Enumerable.Intersect(current, target).Count() == target.Count();
        }

        public static T firstOrDefault<T>(this IEnumerable<T> source, T defaultValue) {
            foreach (var value in source) {
                return value;
            }
            return defaultValue;
        }
        
        public static T firstOrDefault<T>(this IEnumerable<T> source, Predicate<T> predicate, T defaultValue) {
            foreach (var value in source) {
                if (predicate.Invoke(value)) return value;
            }
            return defaultValue;
        }
    }
}
