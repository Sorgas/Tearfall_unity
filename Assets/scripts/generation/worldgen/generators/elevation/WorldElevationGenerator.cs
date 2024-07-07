using System;
using UnityEngine;
using util;
using util.geometry.bounds;
using Random = UnityEngine.Random;

namespace generation.worldgen.generators.elevation {
public class WorldElevationGenerator : WorldGenerator {
    private float[,] elevation;
    private IntBounds2 bounds;

    private float xOffset;
    private float yOffset;

    public WorldElevationGenerator() {
        name = "WorldElevationGenerator";
    }

    public override void generate() {
        log("generating world elevation");
        xOffset = Random.value * 10000;
        yOffset = Random.value * 10000;
        elevation = container.elevation;
        bounds = new IntBounds2(0, 0, config.size - 1, config.size - 1);
        float baseScale = selectPerlinScale(config.size);
        float scaleFactor = 2.5f;
        float worldSizeFactor = (float)config.maxWorldSize / config.size;
        // addElevation(0.01f * worldSizeFactor, 1f);
        addElevation(baseScale, 0.9f);
        baseScale *= scaleFactor;
        addElevation(baseScale, 0.15f);
        baseScale *= scaleFactor;
        addElevation(baseScale, 0.12f);
        baseScale *= scaleFactor;
        addElevation(baseScale, 0.04f);
        // addElevation(0.12f, 0.2f);
        // addElevation(0.3f, 0.1f);
        // addElevation(0.4f, 0.06f);
        lowerBorders();
        normalizeElevation();
        // hack. noise generator always has 0 in (0,0)
        // container.elevation[0, 0] = (elevation[0, 1] + elevation[1, 1] + elevation[1, 0]) / 3f;
    }

    private void addElevation(float scale, float amplitude) {
        Debug.Log(scale);
        bounds.iterate((x, y) => { elevation[x, y] += Mathf.PerlinNoise(xOffset + (x * scale), yOffset + (y * scale)) * amplitude; });
    }

    // Decreases elevation in a circle near borders, to create ocean on map sides
    private void lowerBorders() {
        float mapRadius = (float)(config.size * Math.Sqrt(2) / 2f);
        bounds.iterate((x, y) => {
            float distance = getAbsoluteDistanceToCenter(x, y) / mapRadius;
            float a = 0.6f;
            if (distance > a) {
                elevation[x, y] = elevation[x, y] * (1 - distance) / (1 - a);
            }
        });
    }

    // Counts distance from map center to given point
    private float getAbsoluteDistanceToCenter(int x, int y) {
        float dx = x - config.size / 2f;
        float dy = y - config.size / 2f;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    // Modifies elevation map to be within [0,1]
    private void normalizeElevation() {
        bounds.iterate((x, y) => { elevation[x, y] = elevation[x, y] * (config.maxElevation - config.minElevation) + config.minElevation; });
    }

    private float selectPerlinScale(int size) {
        if (size == 10) return 0.06f;
        if (size == 50) return 0.06f;
        if (size == 100) return 0.03f;
        if (size == 150) return 0.0235f;
        if (size == 200) return 0.02f;
        if (size == 250) return 0.0175f;
        throw new GameException("Unsupported world size");
    }
}
}