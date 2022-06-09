using System.Collections.Generic;
using UnityEngine;

namespace util {
    // byte array which counts number of each byte value in array
    public class UtilByteArrayWithCounter : UtilByteArray {
        public Dictionary<byte, int> sizes = new();

        public UtilByteArrayWithCounter(int xSize, int ySize, int zSize) : base(xSize, ySize, zSize) {
            sizes.Add(0, xSize * ySize * zSize); // init counter
        }

        public UtilByteArrayWithCounter(Vector3Int size) : this(size.x, size.y, size.z) { }

        public new void set(int x, int y, int z, int value) {
            byte oldValue = get(x, y, z);
            base.set(x, y, z, value);
            updateMap(x, y, z, oldValue);
        }

        public new void change(int x, int y, int z, byte delta) {
            byte oldValue = get(x, y, z);
            base.change(x, y, z, delta);
            updateMap(x, y, z, oldValue);
        }

        private void updateMap(int x, int y, int z, byte oldValue) {
            // increase counter for new value
            byte newValue = get(x, y, z);
            if (sizes.ContainsKey(newValue)) {
                sizes[newValue]++;
            } else {
                sizes.Add(newValue, 1);
            }
            // decrease or remove counter for old value
            if (sizes[oldValue] < 2) {
                sizes.Remove(oldValue);
            } else {
                sizes[oldValue]--;
            }
        }
    }
}
