using System.Collections.Generic;

namespace util.lang.extension {
    public static class ListExtensions {
        public static T removeAndGet<T>(this List<T> source, int index) {
            T element = source[index];
            source.RemoveAt(index);
            return element;
        }

        public static string toString<T>(this List<T> source) {
            return "[" + string.Join(", ", source) + "]";
        }
    }
}
