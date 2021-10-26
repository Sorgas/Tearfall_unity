using UnityEngine;

namespace util.lang {
    public static class GameObjectExtension {

        public static bool hasComponent<T>(this GameObject flag)where T : Component{
            return flag.GetComponent<T> () != null;
        }
    }
}
