namespace util.geometry {
    public class ValueRangeInt {
        public int min;
        public int max;

        public ValueRangeInt(int min, int max) => set(min, max);

        public ValueRangeInt() : this(0, 0) { }

        public void set(int min, int max) {
            this.min = min;
            this.max = max;
        }

        public int random() => ((int)UnityEngine.Random.value * (max - min)) + min;

        public bool check(int value) {
            return value >= min && value <= max;
        }

        public int clamp(int value) {
            return value < min
                ? min
                : value > max
                    ? max
                    : value;
        }
    }
}