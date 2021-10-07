namespace util.lang {
    public class Singleton<T> where T : Singleton<T>, new() {
        public static T instance;
        public static object lockObject = new object();

        public static T get() {
            if (instance == null) {
                lock (lockObject) {
                    instance = new T();
                }
            }
            return instance;
        }
    }
}