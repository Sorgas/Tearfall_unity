using Assets.scripts.enums;
using Assets.scripts.util.geometry;
using UnityEngine;

namespace Assets.scripts.generation.localgen {
    public class LocalElevationGenerator : LocalGenerator {
        private LocalGenContainer container;
        private IntBounds2 bounds = new IntBounds2();
        private float xOffset;
        private float yOffset;

        // TODO take in account elevation of surrounding tiles
        public override void generate() {
            LocalGenConfig config = GenerationState.get().localGenConfig;
            container = GenerationState.get().localGenContainer;
            IntVector2 location = config.location;
            float worldElevation = GenerationState.get().world.worldMap.elevation[location.x, location.y];
            int localElevation = (int)(worldElevation * config.worldToLocalElevationModifier) + 1;
            xOffset = Random.value * 10000;
            yOffset = Random.value * 10000;
            bounds.set(0, 0, config.areaSize - 1, config.areaSize - 1);
            Debug.Log("generating elevation");
            addElevation(5, 0.5f, 0.05f, 6f);
            // addElevation(6, 0.5f, 0.015f, 2f);
            // addElevation(7, 0.5f, 0.03f, 1f);
            
            Debug.Log("world elevation " + worldElevation);
            Debug.Log("elevation " + localElevation);
            for (int x = 0; x < config.areaSize; x++) {
                for (int y = 0; y < config.areaSize; y++) {
                    for (int z = 0; z < container.heightsMap[x,y]; z++) {
                        container.localMap.blockType.set(x, y, z, BlockTypeEnum.WALL.CODE);
                    }
                }
            }
        }

        private void addElevation(int octaves, float roughness, float scale, float amplitude) {
            bounds.iterate((x, y) => { container.heightsMap[x, y] += Mathf.PerlinNoise(xOffset + x * scale, yOffset + y * scale) * amplitude; });
        }
    }
}
