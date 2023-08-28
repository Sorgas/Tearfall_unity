using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace util.geometry.bounds {
public class IntBounds3 : IntBounds2 {
    public int minZ;
    public int maxZ;

    public IntBounds3(Vector3Int pos1, Vector3Int pos2) : this(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z) { }

    public IntBounds3() : this(0, 0, 0, 0, 0, 0) { }

    public IntBounds3(IntBounds3 source) : this(source.minX, source.minY, source.minZ, source.maxX, source.maxY, source.maxZ) { }

    public IntBounds3(Vector3Int c, int x, int y, int z) : this(c.x - x, c.y - y, c.z - z, c.x + x, c.y + y, c.z + z) { }

    public IntBounds3(int minX, int minY, int minZ, int maxX, int maxY, int maxZ) => set(minX, minY, minZ, maxX, maxY, maxZ);

    public bool isIn(IntVector3 vector) => isIn(vector.x, vector.y, vector.z);

    public bool isIn(Vector3 vector) => isIn(vector.x, vector.y, vector.z);

    public bool isIn(float x, float y, float z) => base.isIn(x, y) && z <= maxZ && z >= minZ;

    public void set(Vector3Int pos1, Vector3Int pos2) => set(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z);

    public void set(int minX, int minY, int minZ, int maxX, int maxY, int maxZ) {
        base.set(minX, minY, maxX, maxY);
        this.minZ = Math.Min(minZ, maxZ);
        this.maxZ = Math.Max(minZ, maxZ);
    }

    public IntBounds3 set(IntBounds3 source) {
        base.set(source);
        minZ = Math.Min(source.minZ, source.maxZ);
        maxZ = Math.Max(source.minZ, source.maxZ);
        return this;
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
    public void extendTo(Vector3Int position) => extendTo(position.x, position.y, position.z);

    public void extendTo(int x, int y, int z) {
        maxX = Math.Max(maxX, x);
        minX = Math.Min(minX, x);
        maxY = Math.Max(maxY, y);
        minY = Math.Min(minY, y);
        maxZ = Math.Max(maxZ, z);
        minZ = Math.Min(minZ, z);
    }

    public void iterate(Action<Vector3Int> consumer) => iterate((x, y, z) => consumer.Invoke(new Vector3Int(x, y, z)));

    public void iterate(Action<int, int, int> consumer) {
        for (int x = minX; x <= maxX; x++) {
            for (int y = minY; y <= maxY; y++) {
                for (int z = minZ; z <= maxZ; z++) {
                    consumer.Invoke(x, y, z);
                }
            }
        }
    }

    public void iterateX(Action<int> action) => iterateSingle(action, minX, maxX);

    public void iterateY(Action<int> action) => iterateSingle(action, minY, maxY);

    public void iterateZ(Action<int> action) => iterateSingle(action, minZ, maxZ);

    private void iterateSingle(Action<int> action, int min, int max) {
        for (int i = min; i <= max; i++) {
            action.Invoke(i);
        }
    }

    // returns true, if validationFunction is true for all positions
    // returns false, if validationFunction is false for any in-bounds position
    public bool validate(Func<int, int, int, bool> validationFunction) {
        for (int x = minX; x <= maxX; x++) {
            for (int y = minY; y <= maxY; y++) {
                for (int z = minZ; z <= maxZ; z++) {
                    if (!validationFunction.Invoke(x, y, z)) return false;
                }
            }
        }
        return true;
    }

    public IntBounds3 normalizeBounds(IntBounds3 bounds) {
        bounds.minX = Mathf.Max(bounds.minX, minX);
        bounds.maxX = Mathf.Min(bounds.maxX, maxX);
        bounds.minY = Mathf.Max(bounds.minY, minY);
        bounds.maxY = Mathf.Min(bounds.maxY, maxY);
        bounds.minZ = Mathf.Max(bounds.minZ, minZ);
        bounds.maxZ = Mathf.Min(bounds.maxZ, maxZ);
        return bounds;
    }

    // modifies vector to be included into bounds with minimal change
    public Vector3Int putInto(Vector3Int vector) {
        vector.x = Math.Min(maxX, Math.Max(minX, vector.x));
        vector.y = Math.Min(maxY, Math.Max(minY, vector.y));
        vector.z = Math.Min(maxZ, Math.Max(minZ, vector.z));
        return vector;
    }

    // returns outside and adjacent cells
    public List<Vector3Int> getExternalBorders(bool sameLevel) {
        List<Vector3Int> result = new();
        for (int x = minX - 1; x <= maxX + 1; x++) {
            result.Add(new Vector3Int(x, minY - 1, 0));
            result.Add(new Vector3Int(x, maxY + 1, 0));
        }
        for (int y = minY; y <= maxY; y++) {
            result.Add(new Vector3Int(minX - 1, y, 0));
            result.Add(new Vector3Int(maxX + 1, y, 0));
        }
        return result;
        // var result = new List<Vector3Int>();
        // if (sameLevel) {
        //     for (int x = minX - 1; x <= maxX + 1; x++) {
        //         for (int y = minY - 1; y <= maxY + 1; y++) {
        //             result.Add(new Vector3Int(x, y, minZ - 1));
        //             result.Add(new Vector3Int(x, y, maxZ + 1));
        //         }
        //     }
        // }
        // for (int z = minZ; z <= maxZ; z++) {
        //     for (int x = minX - 1; x <= maxX + 1; x++) {
        //         result.Add(new Vector3Int(x, minY - 1, z));
        //         result.Add(new Vector3Int(x, maxY + 1, z));
        //     }
        //     for (int y = minY; y <= maxY + 1; y++) {
        //         result.Add(new Vector3Int(minX - 1, y, z));
        //         result.Add(new Vector3Int(maxX + 1, y + 1, z));
        //     }
        // }
        // return result;
    }

    public List<Vector3Int> toList() {
        List<Vector3Int> result = new();
        iterate(pos => result.Add(pos));
        return result;
    }
    
    public IntBounds3 clone() {
        return new IntBounds3(this);
    }

    public bool isSingleTile() {
        return minX == maxX && minY == maxY && minZ == maxZ;
    }

    public Vector3Int getStart() {
        return new Vector3Int(minX, minY, minZ);
    }

    public new string toString() {
        return "Int3dBounds{" + " " + minX + " " + minY + " " + minZ + " " + maxX + " " + maxY + " " + maxZ + '}';
    }
}
}