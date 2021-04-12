using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.util.geometry;

namespace Assets.scripts.generation.localgen {
    public class LocalGenConfig {
        public IntVector2 location; // position on WorldMap
        public float seaLevel = 0.5f;
        public int worldToLocalElevationModifier = 10;
        public int airLayersAboveGround = 3;
        public int areaSize = 8;
        public int[] sublayerMaxCount = { 5, 5, 6, 6 };
        public int[] sublayerMinThickness = { 4, 4, 6, 8 };
        public int minCaveLayerHeight = 10;
        public int maxCaveLayerHeight = 20;
        public int localElevation;
    }
}
