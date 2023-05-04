using System;

namespace util.lang {
    // stores some read only variable. On first read calculates it with given function, and then returns calculated value.
    public class CachedVariable<T> {
        private readonly Func<T> initializer;
        private T _value;
        private bool initialized = false;
        public T value {
            get {
                if (!initialized) {
                    _value = initializer.Invoke();
                    initialized = true;
                }
                return _value;
            }
        }

        public CachedVariable(Func<T> initializer) {
            this.initializer = initializer;
        }
    }
}