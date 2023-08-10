namespace util.lang {
    public abstract class Singleton<T> where T : Singleton<T>, new() {
        public static T instance;
        public static object lockObject = new();
        protected ToggleableLogger logger = new("Singleton");
        
        public static T get() {
            if (instance == null) {
                lock (lockObject) {
                    instance = new T();
                    instance.init();
                }
            }
            return instance;
        }

        protected virtual void init() { }
    }
}