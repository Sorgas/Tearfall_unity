using Assets.scripts.enums;
using Assets.scripts.util.geometry;
using UnityEngine;

namespace Assets.scripts.generation.localgen {
    public class LocalElevationGenerator : LocalGenerator {

        // TODO take in account elevation of surrounding tiles
        public override void generate() {
            LocalGenConfig config = GenerationState.get().localGenConfig;
            LocalGenContainer container = GenerationState.get().localGenContainer;
            IntVector2 location = config.location;
            float worldElevation = GenerationState.get().world.worldMap.elevation[location.x, location.y];
            Debug.Log("world elevation " + worldElevation);
            int localElevation = (int)(worldElevation * config.worldToLocalElevationModifier) + 1;
            Debug.Log("elevation " + localElevation);
            for (int x = 0; x < config.areaSize; x++) {
                for (int y = 0; y < config.areaSize; y++) {
                    for (int z = 0; z < localElevation; z++) {
                        container.localMap.blockType.set(x, y, z, BlockTypeEnum.WALL.CODE);
                    }
                }
            }
        }
    }
}
