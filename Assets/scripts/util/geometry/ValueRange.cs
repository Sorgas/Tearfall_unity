using Unity.Mathematics;

namespace Assets.scripts.util.geometry {
    //Represents continuous range of a value.
    public class ValueRange {
        public float min;
        public float max;

        public ValueRange(float min, float max) {
            this.min = min;
            this.max = max;
        }

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