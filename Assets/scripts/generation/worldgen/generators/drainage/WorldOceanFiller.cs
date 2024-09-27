using UnityEngine;

namespace generation.worldgen.generators.drainage {
    public class WorldOceanFiller : AbstractWorldGenerator {
        private float seaLevel;
        
        protected override void generateInternal() {
            seaLevel = config.seaLevel;
            float oceanCount = 0;
            int size = config.size;
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    if (container.elevation[x, y] < seaLevel) oceanCount++;
                }
            }
            Debug.Log("sea tiles:" + oceanCount);
        }
    }
}