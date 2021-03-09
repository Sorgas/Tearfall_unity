using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.util.geometry {
    public class IntBounds3 : IntBounds2 {
        public int minZ;
        public int maxZ;

        public IntBounds3(IntVector3 pos1, IntVector3 pos2) : this(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z) { }

        public IntBounds3() : this(0, 0, 0, 0, 0, 0) { }

        public IntBounds3(IntBounds3 source) : this(source.minX, source.minY, source.minZ, source.maxX, source.maxY, source.maxZ) { }

        public IntBounds3(int minX, int minY, int minZ, int maxX, int maxY, int maxZ) {
            set(minX, minY, minZ, maxX, maxY, maxZ);
        }

        public bool isIn(IntVector3 position) {
            return isIn(position.x, position.y, position.z);
        }

        public bool isIn(int x, int y, int z) {
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

        public void iterate(Action<IntVector3> consumer) {
            iterate((x, y, z) => consumer.Invoke(new IntVector3(x, y, z)));
        }

        public void iterate(Action<int, int, int> consumer) {
            for (int x = minX; x <= maxX; x++) {
                for (int y = maxY; y >= minY; y--) {
                    for (int z = minZ; z <= maxZ; z++) {
                        consumer.Invoke(x, y, z);
                    }
                }
            }
        }

        public IntBounds3 clone() {
            return new IntBounds3(this);
        }

        public string toString() {
            return "Int3dBounds{" + " " + minX + " " + minY + " " + minZ + " " + maxX + " " + maxY + " " + maxZ + '}';
        }
    }
}
