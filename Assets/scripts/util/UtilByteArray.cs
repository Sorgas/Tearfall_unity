using Assets.scripts.util.geometry;
using UnityEngine;

namespace Assets.scripts.util {
    public class UtilByteArray {
        protected byte[,,] array;
        IntVector3 size;

        public UtilByteArray(int xSize, int ySize, int zSize) {
            size = new IntVector3(xSize, ySize, zSize);
            array = new byte[xSize, ySize, zSize];
        }

        public byte get(int x, int y, int z) {
            return array[x, y, z];
        }

        public byte get(IntVector3 position) {
            return get(position.x, position.y, position.z);
        }

        public byte get(Vector3Int position) {
            return get(position.x, position.y, position.z);
        }

        public void set(int x, int y, int z, int value) {
            array[x, y, z] = (byte)value;
        }

        public void set(Vector3Int position, int value) {
            set(position.x, position.y, position.z, value);
        }

        public void change(int x, int y, int z, byte delta) {
            array[x, y, z] += delta;
        }

        public bool withinBounds(int x, int y, int z) {
            return x >= 0 && y >= 0 && z >= 0 && x < size.x && y < size.y && z < size.z;
        }
    }
}
