﻿using Assets.scripts.util.geometry;
using UnityEngine;

// fills heights map in local gen container. properties selected based on world cell's biome
namespace Assets.scripts.generation.localgen {
    public class LocalElevationGenerator : LocalGenerator {
        private LocalGenContainer container;
        private IntBounds2 bounds = new IntBounds2();
        private float xOffset;
        private float yOffset;

        // TODO take in account elevation of surrounding tiles
        public override void generate() {
            Debug.Log("generating elevation");
            LocalGenConfig config = GenerationState.get().localGenConfig;
            container = GenerationState.get().localGenContainer;
            IntVector2 location = config.location;
            bounds.set(0, 0, config.areaSize - 1, config.areaSize - 1);
            // GenerationState.get().world.worldMap.biome;
            createElevation();
            modifyElevation(8, 20);
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
    }
}
