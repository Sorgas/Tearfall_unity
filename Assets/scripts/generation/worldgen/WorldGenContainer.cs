using game.model;
using UnityEngine;
using util.geometry;

namespace generation.worldgen {
    public class WorldGenContainer {
        public float[,] elevation;
        public float[,] drainage;
        public float[,] summerTemperature;
        public float[,] winterTemperature;
        public float[,] rainfall;
        public IntVector2[,] rivers;
        public IntVector2[,] brooks;
        public IntVector2[,] debug;
        public int[,] biome;

        public int size;
        // public Random random;

        public WorldGenContainer() {
            
            WorldGenConfig config = GenerationState.get().worldGenConfig;
            this.size = config.size;
            Random.InitState(config.seed);
            elevation = new float[size, size];
            drainage = new float[size, size];
            summerTemperature = new float[size, size];
            winterTemperature = new float[size, size];
            rainfall = new float[size, size];
            biome = new int[size, size];

            rivers = new IntVector2[size, size];
            brooks = new IntVector2[size, size];
            debug = new IntVector2[size, size];
        }

        // collect intermediate results into worldMap
        public WorldMap createWorldMap() {
            WorldMap worldMap = new WorldMap(size);
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    worldMap.elevation[x, y] = elevation[x, y];
                }
            }
            return worldMap;
        }
    }
}