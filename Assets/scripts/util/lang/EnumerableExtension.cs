using System.Collections.Generic;
using System.Linq;

namespace Assets.scripts.util.lang {
    public static class EnumerableExtension {

        public static bool includesAll<T>(this IEnumerable<T> current, IEnumerable<T> target) {
            return Enumerable.Intersect(current, target).Count() == target.Count();
        }
    }
}
