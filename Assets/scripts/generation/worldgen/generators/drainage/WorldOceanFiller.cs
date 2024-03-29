using game.model;
using UnityEngine;

namespace generation.worldgen.generators.drainage {
    public class WorldOceanFiller : WorldGenerator {
        private float seaLevel;

        public WorldOceanFiller() {
            seaLevel = GenerationState.get().worldGenConfig.seaLevel;
        }

        public override void generate() {
            WorldMap map = GameModel.get().world.worldModel.worldMap;
            float oceanCount = 0;
            int size = GenerationState.get().worldGenConfig.size;
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    if (map.elevation[x, y] < seaLevel) oceanCount++;
                }
            }
            Debug.Log("sea tiles:" + oceanCount);
        }
    }
}