using System;

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
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }
        }

        public static bool isHorisontal(Orientations orientation) {
            return orientation == Orientations.E || orientation == Orientations.W;
        }
    }
    
}