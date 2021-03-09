using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.util.geometry {
    class PositionUtil {
        public static List<IntVector3> fourNeighbour;

        // 8 on same level except center
        public static List<IntVector3> allNeighbour;

        public static List<IntVector3> waterflow;

        public static List<IntVector3> all = new List<IntVector3>();

        static PositionUtil() {
            // four orthogonally adjacent
            fourNeighbour = new List<IntVector3>{
                new IntVector3(1, 0, 0),
                new IntVector3(0, 1, 0),
                new IntVector3(-1, 0, 0),
                new IntVector3(0, -1, 0)};

            allNeighbour = new List<IntVector3>(fourNeighbour);
            // four diagonally adjacent
            allNeighbour.Add(new IntVector3(1, 1, 0));
            allNeighbour.Add(new IntVector3(1, -1, 0));
            allNeighbour.Add(new IntVector3(-1, 1, 0));
            allNeighbour.Add(new IntVector3(-1, -1, 0));

            waterflow = new List<IntVector3>(allNeighbour);
            // upper and lower
            waterflow.Add(new IntVector3(0, 0, -1));
            waterflow.Add(new IntVector3(0, 0, 1));

            for (int z = -1; z <= 1; z++) {
                for (int y = -1; y <= 1; y++) {
                    for (int x = -1; x <= 1; x++) {
                        if (x != 0 || y != 0 || z != 0) all.Add(new IntVector3(x, y, z));
                    }
                }
            }
        }
    }
}
