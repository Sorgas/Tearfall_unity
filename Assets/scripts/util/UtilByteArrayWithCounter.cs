using System.Collections.Generic;
using UnityEngine;

namespace util {
    // byte array which counts number of each byte value in array
    public class UtilByteArrayWithCounter : UtilByteArray {
        public Dictionary<byte, int> numbers;

        public UtilByteArrayWithCounter(int xSize, int ySize, int zSize) : base(xSize, ySize, zSize) {
            numbers = new Dictionary<byte, int>();
            numbers.Add(0, xSize * ySize * zSize); // init counter
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
            if (numbers.ContainsKey(newValue)) {
                numbers[newValue]++;
            } else {
                numbers[newValue] = 1;
            }

            // decrease or remove counter for old value
            int old = numbers[oldValue];
            if (old < 2) {
                numbers.Remove(oldValue);
            } else {
                old--;
            }
        }
    }
}
