using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.util.lang {
    public class EnumerableUtil {

        public static bool includesAll<T>(IEnumerable<T> first, IEnumerable<T> second) {
            return Enumerable.Intersect(first, second).Count() == second.Count();
        }
    }
}
