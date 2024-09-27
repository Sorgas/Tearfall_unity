using System;
using UnityEngine;

namespace generation.worldgen.generators.temperature {
public class WorldTemperatureGenerator : AbstractWorldGenerator {
    private float polarLineWidth; //temperature in it is always minimal
    private float equatorLineWidth; //temperature in it is always maximum
    private float maxYearTemperature; // max summer temperature
    private float minYearTemperature; // min winter temperature
    private float maxSummerTemperature;
    private float minWinterTemperature;
    private float[,] yearTemperature;
    private float elevationInfluence;

    protected override void generateInternal() {
        init();
        createGradient();
        addNoiseAndElevation();
        ensureBounds();
        renderTemperature();
    }

    private void init() {
        polarLineWidth = config.polarLineWidth;
        equatorLineWidth = config.equatorLineWidth;
        maxSummerTemperature = config.maxTemperature;
        minWinterTemperature = config.minTemperature;
        maxYearTemperature = maxSummerTemperature - config.seasonalDeviation;
        minYearTemperature = minWinterTemperature + config.seasonalDeviation;
        yearTemperature = new float[config.size, config.size];
        elevationInfluence = config.elevationInfluence;
    }

    // Creates gradient of temperature. Gradient is mirrored on equator.
    private void createGradient() {
        int height2 = config.size / 2;
        float polarBorder = (float)Math.Round(config.size * polarLineWidth); // y of polar border
        float equatorBorder = (float)Math.Round(config.size * (0.5f - equatorLineWidth)); // y of equator border
        for (int y = 0; y < height2; y++) {
            float temp = maxYearTemperature;
            if (y < polarBorder) {
                temp = minYearTemperature;
            } else if (y < equatorBorder) {
                temp = minYearTemperature + ((y - polarBorder) / (equatorBorder - polarBorder)) * (maxYearTemperature - minYearTemperature);
            }
            for (int x = 0; x < config.size; x++) {
                yearTemperature[x, y] = temp;
                yearTemperature[x, config.size - y - 1] = temp;
            }
        }
    }

    /**
     * Adds noise to temperature map and lowers temperature on high places.
     */
    private void addNoiseAndElevation() {
        //TODO add coastal and continental climates difference
        float seaLevel = config.seaLevel;
        // PerlinNoiseGenerator noiseGen = new PerlinNoiseGenerator();
        // float[][] noise = noiseGen.generateOctavedSimplexNoise(config.size, config.size, 7, 0.6f, 0.006f);
        for (int x = 0; x < config.size; x++) {
            for (int y = 0; y < config.size; y++) {
                float elevation = container.elevation[x, y] > seaLevel ? container.elevation[x, y] : 0; // sea depth counts as 0 elevation. max elevation is 1
                yearTemperature[x, y] = yearTemperature[x, y] + (Mathf.PerlinNoise(x, y) * 4) - (40f * elevationDelta(elevation - seaLevel));
            }
        }
    }

    /**
     * Counts summer and winter temperature, and saves in to container.
     */
    private void renderTemperature() {
        float max = 0;
        float min = 0;
        for (int x = 0; x < config.size; x++) {
            for (int y = 0; y < config.size; y++) {
                container.summerTemperature[x, y] = yearTemperature[x, y] + config.seasonalDeviation;
                container.winterTemperature[x, y] = yearTemperature[x, y] - config.seasonalDeviation;
                max = Math.Max(yearTemperature[x, y], max);
                min = Math.Min(yearTemperature[x, y], min);
            }
        }
        Debug.Log("max temp = " + max + " min temp = " + min);
    }

    /**
     * Guarantees that temperature will be within bounds.
     */
    private void ensureBounds() {
        for (int x = 0; x < config.size; x++) {
            for (int y = 0; y < config.size; y++) {
                float rainfall = yearTemperature[x, y];
                yearTemperature[x, y] = rainfall > maxYearTemperature ? maxYearTemperature : (rainfall < minYearTemperature ? minYearTemperature : rainfall);
            }
        }
    }

    /**
     * Counts temperature decreasing rate for elevation.
     * @param elevation
     * @return
     */
    private float elevationDelta(float elevation) {
        float value = (float)(Math.Pow(2, elevation * elevationInfluence) / (elevationInfluence * elevationInfluence));
        return value;
    }
}
}