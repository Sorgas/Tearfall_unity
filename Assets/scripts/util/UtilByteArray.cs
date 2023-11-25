using UnityEngine;

namespace util {
// 3d array of bytes
public class UtilByteArray {
        protected byte[,,] array;
        Vector3Int size;

        public UtilByteArray(int xSize, int ySize, int zSize) {
            size = new Vector3Int(xSize, ySize, zSize);
            array = new byte[xSize, ySize, zSize];
        }

        public UtilByteArray(Vector3Int size) : this(size.x, size.y, size.z) { }

        public byte get(int x, int y, int z) => array[x, y, z];

        public byte get(Vector3Int position) => get(position.x, position.y, position.z);

        public void set(int x, int y, int z, int value) => setImpl(x, y, z, value);

        public void set(Vector3Int position, int value) => set(position.x, position.y, position.z, value);

        protected virtual void changeImpl(int x, int y, int z, byte delta) {
            array[x, y, z] += delta;
        }

        public bool withinBounds(int x, int y, int z) => x >= 0 && y >= 0 && z >= 0 && x < size.x && y < size.y && z < size.z;

        protected virtual void setImpl(int x, int y, int z, int value) {
            array[x, y, z] = (byte)value;
        }
    }
}