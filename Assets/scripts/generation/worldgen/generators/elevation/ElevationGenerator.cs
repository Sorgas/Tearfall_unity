using System;
using Assets.scripts.util.geometry;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.scripts.generation.worldgen.generators.elevation {
    public class ElevationGenerator : WorldGenerator {
        private int size;
        private float[,] elevation;
        private IntBounds2 bounds;
        private WorldGenConfig config;
        private WorldGenContainer container;

        private float xOffset;
        private float yOffset;

        public override void generate(WorldGenConfig config, WorldGenContainer container) {
            this.container = container;
            xOffset = Random.value * 10000;
            yOffset = Random.value * 10000;
            size = config.size;
            elevation = new float[size, size];
            bounds = new IntBounds2(0, 0, size - 1, size - 1);
            Debug.Log("generating elevation");
            addElevation(5, 0.5f, 0.005f, 0.7f);
            addElevation(6, 0.5f, 0.015f, 0.2f);
            addElevation(7, 0.5f, 0.03f, 0.1f);
            lowerBorders();
            normalizeElevation();
            // hack. noise generator always has 0 in (0,0)
            // container.elevation[0, 0] = (elevation[0, 1] + elevation[1, 1] + elevation[1, 0]) / 3f;
        }

        private void addElevation(int octaves, float roughness, float scale, float amplitude) {
            bounds.iterate((x, y) => { elevation[x, y] += Mathf.PerlinNoise(xOffset + x * scale, yOffset + y * scale) * amplitude; });
        }

        /**
        * Decreases elevation in a circle near borders, to create ocean on map sides
        */
        private void lowerBorders() {
            float mapRadius = (float) (size * Math.Sqrt(2) / 2f);
            bounds.iterate((x, y) => {
                float distance = Math.Min(1, -10f / 4 * ((getAbsoluteDistanceToCenter(x, y) - mapRadius) / mapRadius));
                elevation[x, y] = ((elevation[x, y] + 1) * (distance)) - 1f;
            });
        }

        /**
        * Counts distance from map center to given point
        */
        private float getAbsoluteDistanceToCenter(int x, int y) {
            float dx = x - size / 2f;
            float dy = y - size / 2f;
            return (float) Math.Sqrt(dx * dx + dy * dy);
        }

        /**
        * Modifies elevation map to be within [0,1]
        */
        private void normalizeElevation() {
            bounds.iterate((x, y) => container.elevation[x, y] = (elevation[x, y] = (elevation[x, y] + 1) / 2f));
        }
    }
}
