using System;
using UnityEngine;

namespace util.geometry {
    public class IntBounds3 : IntBounds2 {
        public int minZ;
        public int maxZ;
        // private Vector3 cacheVector = new Vector3();

        public IntBounds3(Vector3Int pos1, Vector3Int pos2) : this(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z) { }

        public IntBounds3() : this(0, 0, 0, 0, 0, 0) { }

        public IntBounds3(IntBounds3 source) : this(source.minX, source.minY, source.minZ, source.maxX, source.maxY, source.maxZ) { }

        public IntBounds3(Vector3Int c, int x, int y, int z) : this(c.x - x, c.y - y, c.z - z, c.x + x, c.y + y, c.z + z) { }

        public IntBounds3(int minX, int minY, int minZ, int maxX, int maxY, int maxZ) {
            set(minX, minY, minZ, maxX, maxY, maxZ);
        }

        public new bool isIn(IntVector3 vector) {
            return isIn(vector.x, vector.y, vector.z);
        }

        public bool isIn(Vector3 vector) {
            return isIn(vector.x, vector.y, vector.z);
        }

        public bool isIn(float x, float y, float z) {
            return base.isIn(x, y) && z <= maxZ && z >= minZ;
        }

        public void set(int minX, int minY, int minZ, int maxX, int maxY, int maxZ) {
            base.set(minX, minY, maxX, maxY);
            this.minZ = Math.Min(minZ, maxZ);
            this.maxZ = Math.Max(minZ, maxZ);
        }

        public void set(IntVector3 pos1, IntVector3 pos2) {
            set(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z);
        }

        public void clamp(int minX, int minY, int minZ, int maxX, int maxY, int maxZ) {
            base.clamp(minX, minY, maxX, maxY);
            this.minZ = Math.Max(this.minZ, minZ);
            this.maxZ = Math.Min(this.maxZ, maxZ);
        }

        public Vector3 getInVector(Vector3 position) {
            Vector3 vector = new Vector3();
            if (position.x < minX) vector.x = minX - position.x;
            if (position.x > maxX) vector.x = maxX - position.x;
            if (position.y < minY) vector.y = minY - position.y;
            if (position.y > maxY) vector.y = maxY - position.y;
            if (position.z < minZ) vector.z = minZ - position.z;
            if (position.z > maxZ) vector.z = maxZ - position.z;
            return vector;
        }

        public IntVector3 getInVector(IntVector3 position) {
            IntVector3 vector = new IntVector3();
            if (position.x < minX) vector.x = minX - position.x;
            if (position.x > maxX) vector.x = maxX - position.x;
            if (position.y < minY) vector.y = minY - position.y;
            if (position.y > maxY) vector.y = maxY - position.y;
            if (position.z < minZ) vector.z = minZ - position.z;
            if (position.z > maxZ) vector.z = maxZ - position.z;
            return vector;
        }

        /**
         * Extends bounds so given position becomes included.
         */
        public void extendTo(IntVector3 position) {
            extendTo(position.x, position.y, position.z);
        }

        public void extendTo(int x, int y, int z) {
            maxX = Math.Max(maxX, x);
            minX = Math.Min(minX, x);
            maxY = Math.Max(maxY, y);
            minY = Math.Min(minY, y);
            maxZ = Math.Max(maxZ, z);
            minZ = Math.Min(minZ, z);
        }

        public void iterate(Action<Vector3Int> consumer) {
            iterate((x, y, z) => consumer.Invoke(new Vector3Int(x, y, z)));
        }

        public void iterate(Action<int, int, int> consumer) {
            for (int x = minX; x <= maxX; x++) {
                for (int y = minY; y <= maxY; y++) {
                    for (int z = minZ; z <= maxZ; z++) {
                        consumer.Invoke(x, y, z);
                    }
                }
            }
        }

        // modifies vector to be included into bounds with minimal change
        public Vector3Int putInto(Vector3Int vector) {
            vector.x = Math.Min(maxX, Math.Max(minX, vector.x));
            vector.y = Math.Min(maxY, Math.Max(minY, vector.y));
            vector.z = Math.Min(maxZ, Math.Max(minZ, vector.z));
            return vector;
        }
        
        public IntBounds3 clone() {
            return new IntBounds3(this);
        }

        public new string toString() {
            return "Int3dBounds{" + " " + minX + " " + minY + " " + minZ + " " + maxX + " " + maxY + " " + maxZ + '}';
        }
    }
}
