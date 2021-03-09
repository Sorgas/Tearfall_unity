using UnityEngine;

namespace Assets.scripts.util.geometry
{
public class IntVector2 {
    public int x, y;
    public IntVector2(int x, int y) {
        set(x, y);
    }
    public IntVector2() : this(0, 0) {
    }
    public IntVector2(IntVector2 source) : this(source.x, source.y) {
    }
    public IntVector2(int[] source) : this(source[0], source[1]) {
    }

    // Makes negative components positive. Returns vector with new values of changed components.
    public IntVector2 invertToPositive() {
        return new IntVector2(
            x < 0 ? x = -x : 0,
            y < 0 ? y = -y : 0);
    }

    public IntVector2 add(IntVector2 vector) {
        x += vector.x;
        y += vector.y;
        return this;
    }

    public void set(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public void set(IntVector2 vector) {
        set(vector.x, vector.y);
    }

    public void set(Vector3 vector) {
        set((int)vector.x, (int)vector.y);
    }

    public IntVector2 clone() {
        return new IntVector2(x, y);
    }

    protected bool Equals(IntVector2 other) {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((IntVector2)obj);
    }

    public override int GetHashCode() {
        unchecked {
            return (x * 397) ^ y;
        }
    }

    public override string ToString() {
        return "[" + x + "," + y + "]";
    }
}
}