using System;
using System.Linq;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace util.geometry {

    public static class Vector3IntExtension {
        public static Vector3Int add(this Vector3Int vector, int dx, int dy, int dz) {
            vector.x += dx;
            vector.y += dy;
            vector.z += dz;
            return vector;
        }

        public static Vector3Int set(this Vector3Int vector, int x, int y, int z) {
            vector.Set(x, y, z);
            return vector;
        }

        public static Vector3Int set(this ref Vector3Int vector, Vector3Int value) {
            vector.Set(value.x, value.y, value.z);
            return vector;
        }

        public static bool isNeighbour(this Vector3Int vector, Vector3Int value) {
            Vector3Int result = vector - value;
            return result.x > -2 && result.x < 2 &&
                   result.y > -2 && result.y < 2 &&
                   result.z > -2 && result.z < 2;
        }
        
        public static float fastDistance(this Vector3Int vector, Vector3Int value) {
            return (vector - value).sqrMagnitude;
        }
    }

    // position
    public class IntVector3 {
        public int x;
        public int y;
        public int z;

        public IntVector3() : this(0, 0, 0) {
        }

        public IntVector3(int x, int y, int z) {
            set(x, y, z);
        }

        public IntVector3(float x, float y, float z) {
            this.x = (int)Math.Round(x);
            this.y = (int)Math.Round(y);
            this.z = (int)Math.Round(z);
        }

        public IntVector3(Vector3 vector) {
            this.x = (int)Math.Round(vector.X);
            this.y = (int)Math.Round(vector.Y);
            this.z = (int)Math.Round(vector.Z);
        }

        public static bool operator ==(IntVector3 pos1, IntVector3 pos2) => pos1 != null && pos2 != null && pos1.x == pos2.x && pos1.y == pos2.y && pos1.z == pos2.z;

        public static bool operator !=(IntVector3 pos1, IntVector3 pos2) => pos1 != null && pos2 != null &&
        pos1.x != pos2.x || pos1.y != pos2.y || pos1.z != pos2.z;

        public static IntVector3 operator +(IntVector3 pos1, IntVector3 pos2) => new IntVector3(pos1.x + pos2.x, pos1.y + pos2.y, pos1.z + pos2.z);

        public static IntVector3 add(IntVector3 pos1, IntVector3 pos2) {
            return new IntVector3(pos1.x + pos2.x, pos1.y + pos2.y, pos1.z + pos2.z);
        }

        public static IntVector3 add(IntVector3 pos1, int x, int y, int z) {
            return new IntVector3(pos1.x + x, pos1.y + y, pos1.z + z);
        }

        public static IntVector3 sub(IntVector3 pos1, IntVector3 pos2) {
            return new IntVector3(pos1.x - pos2.x, pos1.y - pos2.y, pos1.z - pos2.z);
        }

        public static IntVector3 sub(IntVector3 pos1, int x, int y, int z) {
            return new IntVector3(pos1.x - x, pos1.y - y, pos1.z - z);
        }

        public IntVector3 add(IntVector3 vector) {
            return add(vector.x, vector.y, vector.z);
        }

        public float getDistance(IntVector3 pos) {
            return getDistance(pos.x, pos.y, pos.z);
        }

        //Real distance.
        public float getDistance(int x, int y, int z) {
            return (float)Math.Sqrt(Math.Pow(this.x - x, 2) + Math.Pow(this.y - y, 2) + Math.Pow(this.z - z, 2));
        }

        // For using in comparators. Coordinates should be positive.
        public int fastDistance(IntVector3 p) {
            return Math.Abs(x - p.x) + Math.Abs(y - p.y) + Math.Abs(z - p.z);
        }
        
        

        public bool isNeighbour(IntVector3 intVector3) {
            IntVector3 result = sub(this, intVector3);
            return result.x > -2 && result.x < 2 &&
                   result.y > -2 && result.y < 2 &&
                   result.z > -2 && result.z < 2;
        }

        public bool isZero() {
            return x == 0 && y == 0 && z == 0;
        }

        public IntVector3 set(IntVector3 intVector3) {
            return set(intVector3.x, intVector3.y, intVector3.z);
        }

        public IntVector3 set(Vector3 vector) {
            return set(Math.Round(vector.X), Math.Round(vector.Y), Math.Round(vector.Z));
        }

        public IntVector3 set(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
            return this;
        }

        public IntVector3 set(double x, double y, double z) {
            return set((int)x, (int)y, (int)z);
        }

        public IntVector3 add(IntVector2 vector) {
            return add(vector.x, vector.y, 0);
        }

        public IntVector3 add(int dx, int dy, int dz) {
            x += dx;
            y += dy;
            z += dz;
            return this;
        }

        protected bool Equals(IntVector3 other) {
            return x == other.x && y == other.y && z == other.z;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IntVector3)obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = x;
                hashCode = (hashCode * 397) ^ y;
                hashCode = (hashCode * 397) ^ z;
                return hashCode;
            }
        }

        public Vector3 toVector3() {
            return new Vector3(x, y, z);
        }

        public String toString() {
            return ("[" + x + " " + y + " " + z + "]");
        }

        public IntVector3[] getNeighbours(IntVector3[] deltas) {
            return deltas.Select(position => add(position, this)).ToArray();
        }
    }
}