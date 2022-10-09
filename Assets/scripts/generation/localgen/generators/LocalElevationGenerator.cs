using UnityEngine;

// fills heights map in local gen container. properties selected based on world cell's biome
namespace generation.localgen.generators {
    public class LocalElevationGenerator : LocalGenerator {
        private float xOffset;
        private float yOffset;

        public LocalElevationGenerator(LocalMapGenerator generator) : base(generator) {}

        // TODO take in account elevation of surrounding tiles
        public override void generate() {
            Debug.Log("generating elevation");
            Vector2Int location = config.location;
            bounds.set(0, 0, config.areaSize - 1, config.areaSize - 1);
            // GenerationState.get().world.worldMap.biome;
            createElevation();
            // TODO base modifier on biome (plains, hills mountain regions)
            modifyElevation(8, container.localElevation);
            logElevation();
        }

        // TODO use different settings for different biomes
        private void createElevation() {
            addElevation(5, 0.5f, 0.05f, 1f);
            // addElevation(6, 0.5f, 0.015f, 2f);
            // addElevation(7, 0.5f, 0.03f, 1f);
        }

        private void addElevation(int octaves, float roughness, float scale, float amplitude) {
            xOffset = Random.value * 10000;
            yOffset = Random.value * 10000;
            bounds.iterate((x, y) => container.heightsMap[x, y] += Mathf.PerlinNoise(xOffset + x * scale, yOffset + y * scale) * amplitude);
        }

        private void modifyElevation(float modifier, int addition) {
            bounds.iterate((x, y) => {
                container.heightsMap[x, y] *= modifier;
                container.heightsMap[x, y] += addition;
            });
        }

        private void logElevation() {
            float min = container.heightsMap[0, 0];
            float max = container.heightsMap[0, 0];
            bounds.iterate((x, y) => {
                float current = container.heightsMap[x, y];
                if(min > current) min = current;
                if(max < current) max = current;
            });
            Debug.Log("elevation generated, min: " + min + ", max: " + max);
        }

        public override string getMessage() {
            return "generating elevation..";
        }
    }
}
