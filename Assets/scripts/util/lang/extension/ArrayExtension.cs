using System;
using System.Collections.Generic;

namespace util.lang.extension {
    public static class ArrayExtension {

        public static T[] subArray<T>(this T[] source, int startIndex) {
            T[] newArray = new T[source.Length - startIndex];
            Array.Copy(source, startIndex, newArray, 0, source.Length - startIndex);
            return newArray;
        }

        public static List<T> subList<T>(this T[] source, int startIndex) {
            return new List<T>(subArray(source, startIndex));
        }
    }
}