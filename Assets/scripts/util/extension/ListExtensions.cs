using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.util.extension {
    public static class ListExtensions {
        public static T RemoveAndGet<T>(this List<T> source, int index) {
            T element = source[index];
            source.RemoveAt(index);
            return element;
        }
    }
}
