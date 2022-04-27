using System.Collections.Generic;

namespace util.lang.extension {
    public static class ListExtensions {
        public static T removeAndGet<T>(this List<T> source, int index) {
            T element = source[index];
            source.RemoveAt(index);
            return element;
        }
    }
}
