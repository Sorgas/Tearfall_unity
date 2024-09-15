using UnityEngine;

namespace types {
public enum Orientations {
    N = 0,
    E = 1,
    S = 2,
    W = 3
}

public enum UnitOrientations {
    N = 0,
    NE = 1,
    E = 2,
    SE = 3,
    S = 4,
    SW = 5,
    W = 6,
    NW = 7
}

public enum SpriteOrientations {
    FR,
    FL,
    BR,
    BL
}

public static class OrientationUtil {
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

    // E, W are horizontal, N, S are vertical
    public static bool isHorizontal(Orientations orientation) {
        return orientation == Orientations.E || orientation == Orientations.W;
    }

    public static Orientations getRandom() {
        int orientation = (int)System.Math.Floor(Random.value * 4);
        if (orientation == 4) orientation = 3;
        return (Orientations)orientation;
    }

    public static Vector3Int getOffset(Orientations orientation) {
        switch (orientation) {
            case Orientations.N: return Vector3Int.up;
            case Orientations.E: return Vector3Int.right;
            case Orientations.S: return Vector3Int.down;
            case Orientations.W: return Vector3Int.left;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(orientation), orientation, null);
        }
    }
}

public static class SpriteOrientationsUtil {
    public static SpriteOrientations byVector(Vector2Int vector) {
        return SpriteOrientations.FR;
    }

    public static bool isRight(SpriteOrientations orientation) {
        return orientation == SpriteOrientations.FR || orientation == SpriteOrientations.BR;
    }
    
    public static bool isFront(SpriteOrientations orientation) {
        return orientation == SpriteOrientations.FR || orientation == SpriteOrientations.FL;
    }
}

public static class UnitOrientationsUtil {
    public static UnitOrientations byVector(Vector3Int vector) {
        if (vector.y > 0) {
            if (vector.x > 0) return UnitOrientations.NE;
            if (vector.x < 0) return UnitOrientations.NW;
            return UnitOrientations.N;
        }
        if(vector.y < 0) {
            if (vector.x > 0) return UnitOrientations.SE;
            if (vector.x < 0) return UnitOrientations.SW;
            return UnitOrientations.S;
        }
        if (vector.x > 0) return UnitOrientations.E;
        if (vector.x < 0) return UnitOrientations.W;
        return UnitOrientations.SW; // default
    }

    public static bool isWest(UnitOrientations orientation) {
        return orientation == UnitOrientations.W || orientation == UnitOrientations.NW || orientation == UnitOrientations.SW;
    }
    
    public static bool isEast(UnitOrientations orientation) {
        return orientation == UnitOrientations.E || orientation == UnitOrientations.NE || orientation == UnitOrientations.SE;
    }
    
    public static bool isNorth(UnitOrientations orientation) {
        return orientation == UnitOrientations.NW || orientation == UnitOrientations.N || orientation == UnitOrientations.NE;
    }
    
    public static bool isSouth(UnitOrientations orientation) {
        return orientation == UnitOrientations.SW || orientation == UnitOrientations.S || orientation == UnitOrientations.SE;
    }
}
}