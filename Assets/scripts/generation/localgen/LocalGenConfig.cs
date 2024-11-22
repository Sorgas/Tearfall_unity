﻿using UnityEngine;

namespace generation.localgen {
// class for storing all parameters for local map generation    
public class LocalGenConfig {
        public int seed;
        public Vector2Int location; // position on WorldMap
        public int airLayersAboveGround = 20;
        public int areaSize = 100;
        public int[] sublayerMaxCount = { 5, 5, 6, 6 };
        public int[] sublayerMinThickness = { 4, 4, 6, 8 };
        public int minCaveLayerHeight = 10;
        public int maxCaveLayerHeight = 20;
        public int minLocalElevation = 30; // for 0 world elevation
        public int maxLocalElevation = 80; // for 1 world elevation
        public string localBiome = "hills"; 
        
        public int elevationVariation = 6; // TODO base on world cell's biome (plains/mountains)
        public float soilThickness = 0.05f; // relative to localElevation TODO base on world cell's moisture (swamps/mountains)
        public int settlerNumber = 1;
        public int forestationLevel = 4; // TODO get from world map
    }
}
