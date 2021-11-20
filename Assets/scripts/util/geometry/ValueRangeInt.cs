using System;

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
        
        public ValueRangeInt setAndNormalize(int value1, int value2) {
            min = Math.Min(value1, value2);
            max = Math.Max(value1, value2);
            return this;
        }

        public void iterate(Action<int> action) {
            for (int i = min; i <= max; i++) {
                action.Invoke(i);
            }
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

        // expands range to include value
        public ValueRangeInt expandTo(int value) {
            if (value < min) min = value;
            if (value > max) max = value;
            return this;
        }

        public string toString() {
            return min + " " + max;
        }
    }
}