using UnityEngine;

namespace types {
    public enum Orientations {
        N = 0,
        E = 1,
        S = 2,
        W = 3
    }

    public class OrientationUtil {
        public static Orientations getNext(Orientations orientation) {
            switch (orientation) {
                case Orientations.N: return Orientations.E;
                case Orientations.E: return Orientations.S;
                case Orientations.S: return Orientations.W;
                case Orientations.W: return Orientations.N;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }
        }

        public static bool isHorizontal(Orientations orientation) {
            return orientation == Orientations.E || orientation == Orientations.W;
        }

        public static Orientations getRandom() {
            int orientation = (int)System.Math.Floor(Random.value * 4);
            if (orientation == 4) orientation = 3;
            return (Orientations)orientation;
        }
    }
}