using System.Collections.Generic;
using UnityEngine;

namespace util {
    // byte array which counts number of each byte value in array
    public class UtilByteArrayWithCounter : UtilByteArray {
        public readonly Dictionary<byte, int> sizes = new();

        public UtilByteArrayWithCounter(int xSize, int ySize, int zSize) : base(xSize, ySize, zSize) {
            sizes.Add(0, xSize * ySize * zSize); // init counter
        }

        public UtilByteArrayWithCounter(Vector3Int size) : this(size.x, size.y, size.z) { }

        protected override void changeImpl(int x, int y, int z, byte delta) {
            byte oldValue = get(x, y, z);
            base.changeImpl(x, y, z, delta);
            updateSizes(x, y, z, oldValue);
        }

        protected override void setImpl(int x, int y, int z, int value) {
            byte oldValue = get(x, y, z);
            base.setImpl(x, y, z, value);
            updateSizes(x, y, z, oldValue);
        }
        
        private void updateSizes(int x, int y, int z, byte oldValue) {
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
