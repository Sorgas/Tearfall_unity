using System;
using UnityEngine;

namespace generation.worldgen.generators.drainage {
public class RainfallGenerator : WorldGenerator {
    private int minRainfall;
    private int maxRainfall;

    private float[,] rainfallBuffer;
    private bool[,] rainfallSet;
    private float elevationFactor = 3f;
    
    public override void generate() {
        Debug.Log("generating rainfall");
        init();
        addMainGradientOnWater();
        fillEmptyIntoBuffer();
        addPerlinNoise();
        ensureBounds();
    }

    private void init() {
        minRainfall = config.minRainfall;
        maxRainfall = config.maxRainfall;
        rainfallBuffer = new float[config.size, config.size];
        rainfallSet = new bool[config.size, config.size];
    }

    // gradient has min on poles and max on equator.
    private void addMainGradientOnWater() {
        float equator = config.size / 2f;
        for (int y = 0; y < config.size; y++) {
            float rainfall = ((-Math.Abs(y - (equator))) / (equator) + 1) * (maxRainfall - minRainfall) + minRainfall;
            for (int x = 0; x < config.size; x++) {
                if (container.elevation[x, y] <= config.seaLevel) {
                    container.rainfall[x, y] = rainfall;
                    rainfallSet[x, y] = true;
                }
            }
        }
    }

    // Fills rainfall above land, starting from coasts.
    private void fillEmptyIntoBuffer() {
        for (int i = 0; i < config.size; i++) {
            for (int x = 0; x < config.size; x++) {
                for (int y = 0; y < config.size; y++) {
                    if (!rainfallSet[x, y] && hasNearRainfall(x, y)) {
                        rainfallBuffer[x, y] = countNearRainfall(x, y);
                        // rainfallBuffer[x, y] *= 0.98f;
                    }
                }
            }
            flushBufferToMap();
        }
    }

    private bool hasNearRainfall(int centerX, int centerY) {
        for (int x = centerX - 1; x <= centerX + 1; x++) {
            for (int y = centerY - 1; y <= centerY + 1; y++) {
                if (x >= 0 && y >= 0 && x < config.size && y < config.size) {
                    if (rainfallSet[x, y]) return true;
                }
            }
        }
        return false;
    }

    private float countNearRainfall(int centerX, int centerY) {
        float rainfall = 0;
        int count = 0;
        for (int x = centerX - 1; x <= centerX + 1; x++) {
            for (int y = centerY - 1; y <= centerY + 1; y++) {
                if (x >= 0 && y >= 0 && x < config.size && y < config.size) {
                    if (rainfallSet[x, y]) {
                        rainfall += container.rainfall[x, y];
                        count++;
                    }
                }
            }
        }
        if (count == 0) return 0; // should never happen
        rainfall /= count;
        rainfall *= (elevationFactor - container.elevation[centerX, centerY]) / elevationFactor;
        return rainfall;
    }

    private void flushBufferToMap() {
        for (int x = 0; x < config.size; x++) {
            for (int y = 0; y < config.size; y++) {
                if (rainfallBuffer[x, y] != 0) {
                    container.rainfall[x, y] = rainfallBuffer[x, y];
                    rainfallBuffer[x, y] = 0;
                    rainfallSet[x, y] = true;
                }
            }
        }
    }

    /**
     * Adds Perlin noise on rainfall map
     */
    private void addPerlinNoise() {
        for (int x = 0; x < config.size; x++) {
            for (int y = 0; y < config.size; y++) {
                container.rainfall[x, y] = (float)Math.Round(container.rainfall[x, y] + Mathf.PerlinNoise(x * 0.3f, y * 0.3f) * 10);
            }
        }
    }

    /**
     * Guarantees that rainfall will be within bounds.
     */
    private void ensureBounds() {
        for (int x = 0; x < config.size; x++) {
            for (int y = 0; y < config.size; y++) {
                float rainfall = container.rainfall[x, y];
                container.rainfall[x, y] = rainfall > maxRainfall ? maxRainfall : (rainfall < minRainfall ? minRainfall : rainfall);
            }
        }
    }
}
}