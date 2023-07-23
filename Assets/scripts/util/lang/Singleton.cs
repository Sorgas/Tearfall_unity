using UnityEngine;

namespace util.lang {
    public abstract class Singleton<T> where T : Singleton<T>, new() {
        public static T instance;
        public static object lockObject = new();

        public static T get() {
            if (instance == null) {
                lock (lockObject) {
                    // Debug.Log("[Singleton]: creating instance of " + typeof(T).Name);
                    instance = new T();
                    instance.init();
                }
            }
            return instance;
        }

        protected virtual void init() { }
    }
}