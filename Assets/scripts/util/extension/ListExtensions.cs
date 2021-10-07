using System.Collections.Generic;

namespace util.extension {
    public static class ListExtensions {
        public static T RemoveAndGet<T>(this List<T> source, int index) {
            T element = source[index];
            source.RemoveAt(index);
            return element;
        }
    }
}
