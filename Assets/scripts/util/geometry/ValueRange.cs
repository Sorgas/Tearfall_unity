namespace util.geometry {
    //Represents continuous range of a value.
    public class ValueRange {
        public float min;
        public float max;

        public ValueRange(float min, float max) => set(min, max);

        public ValueRange() : this(0, 0) { }

        public void set(float min, float max) {
            this.min = min;
            this.max = max;
        }

        public float getRandom() => UnityEngine.Random.value * (max - min) + min;

        public bool check(float value) {
            return value >= min && value <= max;
        }

        public float clamp(float value) {
            return value < min
                ? min
                : value > max
                    ? max
                    : value;
        }
    }
}