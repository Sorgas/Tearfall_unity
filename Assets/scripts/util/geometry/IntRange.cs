namespace util.geometry {
    // range of inclusive borders
    public class IntRange {
        public int max;
        public int min;

        public IntRange(int max, int min) {
            this.max = max;
            this.min = min;
        }

        public bool isIn(int value) {
            return value <= max && value >= min;
        }

        public int putIn(int value) {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}