using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.util.geometry {
    class PositionUtil {
        public static List<Vector3Int> fourNeighbour;

        // 8 on same level except center
        public static List<Vector3Int> allNeighbour;

        public static List<Vector3Int> waterflow;

        public static List<Vector3Int> all = new List<Vector3Int>();

        static PositionUtil() {
            // four orthogonally adjacent
            fourNeighbour = new List<Vector3Int>{
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, -1, 0)};

            allNeighbour = new List<Vector3Int>(fourNeighbour);
            // four diagonally adjacent
            allNeighbour.Add(new Vector3Int(1, 1, 0));
            allNeighbour.Add(new Vector3Int(1, -1, 0));
            allNeighbour.Add(new Vector3Int(-1, 1, 0));
            allNeighbour.Add(new Vector3Int(-1, -1, 0));

            waterflow = new List<Vector3Int>(allNeighbour);
            // upper and lower
            waterflow.Add(new Vector3Int(0, 0, -1));
            waterflow.Add(new Vector3Int(0, 0, 1));

            for (int z = -1; z <= 1; z++) {
                for (int y = -1; y <= 1; y++) {
                    for (int x = -1; x <= 1; x++) {
                        if (x != 0 || y != 0 || z != 0) all.Add(new Vector3Int(x, y, z));
                    }
                }
            }
        }
    }
}
