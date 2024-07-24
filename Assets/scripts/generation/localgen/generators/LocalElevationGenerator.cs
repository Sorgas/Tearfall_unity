using UnityEngine;
using util;

// fills heights map in local gen container. properties selected based on world cell's biome
namespace generation.localgen.generators {
    public class LocalElevationGenerator : LocalGenerator {
        private float xOffset;
        private float yOffset;

        public LocalElevationGenerator(LocalMapGenerator generator) : base(generator) {}

        // TODO take in account elevation of surrounding world tiles
        // TODO calculate worldmap slope direction and incline
        public override void generate() {
            Debug.Log("[LocalElevationGenerator]: generating elevation");
            Vector2Int location = config.location;
            bounds.set(0, 0, config.areaSize - 1, config.areaSize - 1);
            createElevation();
            // TODO base modifier on biome (plains, hills mountain regions)
            modifyElevation(GlobalSettings.localElevation - 4, GlobalSettings.localElevation + 4); 
        }

        // TODO add more biomes
        private void createElevation() {
            string biome = config.localBiome;
            if ("hills".Equals(biome)) {
                createHillElevation();
            } else if ("flat".Equals(biome)) {
                createFlatElevation();
            } else {
                throw new GameException($"Unsupported biome type {biome}");
            }
        }
        
        // rearranges heights map to be in given range
        private void modifyElevation(int min, int max) {
            bounds.iterate((x, y) => {
                container.heightsMap[x, y] *= max - min;
                container.heightsMap[x, y] += min;
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
        }
        
        // creates local elevation values 
        private void createHillElevation() {
            addElevation(0.014f, 0.6f);
            addElevation(0.05f, 0.3f);
            addElevation(0.11f, 0.1f);
        }

        private void createFlatElevation() {
            bounds.iterate((x, y) => container.heightsMap[x, y] = 0.5f);
        }
        
        // more scale -> more detailed noise
        private void addElevation(float scale, float amplitude) {
            xOffset = Random.value * 10000;
            yOffset = Random.value * 10000;
            bounds.iterate((x, y) => container.heightsMap[x, y] += Mathf.PerlinNoise(xOffset + x * scale, yOffset + y * scale) * amplitude);
        }
        
        public override string getMessage() {
            return "generating elevation..";
        }
    }
}
