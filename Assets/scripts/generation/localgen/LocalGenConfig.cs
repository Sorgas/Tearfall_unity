﻿using Assets.scripts.util.geometry;

namespace Assets.scripts.generation.localgen {
    public class LocalGenConfig {
        public IntVector2 location; // position on WorldMap
        public float seaLevel = 0.5f;
        public int airLayersAboveGround = 10;
        public int areaSize = 50;
        public int[] sublayerMaxCount = { 5, 5, 6, 6 };
        public int[] sublayerMinThickness = { 4, 4, 6, 8 };
        public int minCaveLayerHeight = 10;
        public int maxCaveLayerHeight = 20;
        
        public int localElevation = 50; // TODO take from game settings
        public int elevationVariation = 6; // TODO base on world cell's biome (plains/mountains)
        public int soilThickness = 3; // TODO base on world cell's biome (swamps/mountains)
    }
}
