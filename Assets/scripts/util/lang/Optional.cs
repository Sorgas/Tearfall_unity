using System;

namespace Assets.scripts.util.lang {
    public class Optional<T> {
        private T value;

        public Optional(T value) {
            this.value = value;
        }

        public Optional() {
        }

        public Optional<R> map<R>(Func<T, R> mappling) {
            if(value != null) {
                return new Optional<R>(mappling.Invoke(value));
            } else {
                return new Optional<R>();
            }
        }

        public T orElse(T other) {
            return value == null ? other : value;
        }

        public void ifPresent(Action<T> consumer) {
            if (value != null) consumer.Invoke(value);
        }
    }
}
