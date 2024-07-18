using System;
using System.Collections.Generic;
using System.Linq;
using types;
using types.material;
using UnityEngine;
using util.geometry;
using Random = System.Random;

namespace generation.localgen.generators {
// generates several layers of stone and soil materials and flushes them to local map 
public class LocalStoneLayersGenerator : LocalGenerator {
    private ValueRange layerThicknessRange = new();
    private int[,] currentHeight;
    int[,] borderArray;
    private float xOffset;
    private float yOffset;

    // private float effectiveElevation = 0;
    private Dictionary<string, (int, int)> oreDepthRanges = new();

    public LocalStoneLayersGenerator(LocalMapGenerator generator) : base(generator) {
        oreDepthRanges.Add("chalcopyrite", (20, 50));
        oreDepthRanges.Add("magnetite", (25, 45));
        oreDepthRanges.Add("hematite", (20, 35));
        oreDepthRanges.Add("native gold", (10, 25));
        oreDepthRanges.Add("native silver", (15, 35));
        oreDepthRanges.Add("coal", (35, 50));
        debug = true;
    }

    public override void generate() {
        currentHeight = new int[config.areaSize, config.areaSize];
        // borderArray = new int[config.areaSize, config.areaSize];
        bounds.iterate((x, y) => { currentHeight[x, y] = (int)container.heightsMap[x, y]; });
        generateLayers();
    }

    // counts average
    private void countAverageElevation() {
        float average = 0;
        float maxElevation = 0;
        bounds.iterate((x, y) => {
            float elevation = container.heightsMap[x, y];
            average += elevation;
            if (maxElevation < elevation) maxElevation = elevation;
        });
        average /= (config.areaSize * config.areaSize);
        // effectiveElevation = average + (maxElevation - average) / 2;
    }

    private void generateLayers() {
        // TODO get soil thickness from biome
        int soilThickness = (int)Math.Max(GlobalSettings.localElevation * config.soilThickness, 1);
        generateLayer(MaterialMap.get().id("soil"), soilThickness - 1, soilThickness + 1);
        int sedStart = GlobalSettings.localElevation - soilThickness - 1;
        int metStart = createLayerGroup("stone_sedimentary", sedStart);
        int ignStart = createLayerGroup("stone_metamorfic", metStart);
        createLayerGroup("stone_igneous", ignStart);
        generateOre("sedimentary", sedStart, metStart);
        generateOre("sedimentary", metStart, ignStart);
        generateOre("sedimentary", ignStart, 3);
        finalizeBottom(MaterialMap.get().getByTag("stone_igneous")[0].id);
    }

    // generates layers of all materials with tag, returns lower point of layer group
    private int createLayerGroup(string tag, int previousHeight) {
        int groupThickness = (int)(GlobalSettings.localElevation * UnityEngine.Random.Range(0.25f, 0.3f));
        int lowerBorderHeight = previousHeight - groupThickness;
        xOffset = UnityEngine.Random.value * 10000;
        yOffset = UnityEngine.Random.value * 10000;
        List<Material_> materials = MaterialMap.get().getByTag(tag);
        bounds.iterate((x, y) => {
            int targetHeight = lowerBorderHeight - 1 + (int)(Mathf.PerlinNoise(xOffset + x * 0.03f, yOffset + y * 0.03f) * 3); // noised height
            int thickness = (currentHeight[x, y] - targetHeight) / materials.Count;
            for (int z = currentHeight[x, y]; z >= targetHeight; z--) {
                int i = (z - targetHeight) / thickness % materials.Count;
                container.map.blockType.setRaw(x, y, z, BlockTypes.WALL.CODE, materials[i].id);
            }
            currentHeight[x, y] = targetHeight - 1;
        });
        return lowerBorderHeight;
    }

    // generates layer of single material and fills it into map.
    private void generateLayer(int material, int minThickness, int maxThickness) {
        xOffset = UnityEngine.Random.value * 10000;
        yOffset = UnityEngine.Random.value * 10000;
        int thicknessRange = maxThickness - minThickness;
        bounds.iterate((x, y) => {
            int thickness = minThickness + (int)(Mathf.PerlinNoise(xOffset + x * 0.03f, yOffset + y * 0.03f) * thicknessRange);
            int targetHeight = Math.Max(currentHeight[x, y] - thickness, 0);
            if (targetHeight < currentHeight[x, y]) {
                for (int z = currentHeight[x, y]; z >= targetHeight; z--) {
                    container.map.blockType.setRaw(x, y, z, BlockTypes.WALL.CODE, material);
                }
            }
            currentHeight[x, y] = targetHeight;
        });
    }

    // fills all free space in the map bottom with last layer material
    private void finalizeBottom(int material) {
        bounds.iterate((x, y) => {
            if (currentHeight[x, y] >= 0) {
                for (int z = 0; z <= currentHeight[x, y]; z++) {
                    container.map.blockType.setRaw(x, y, z, BlockTypes.WALL.CODE, material);
                }
            }
        });
    }

    public override string getMessage() {
        return "generating stone layers..";
    }

    private void generateOre(string tag, int maxHeight, int minHeight) {
        OreVeinGenerator generator = new();
        xOffset = UnityEngine.Random.value * 10000;
        yOffset = UnityEngine.Random.value * 10000;
        int[,] oreArray = new int[50, 50];
        for (int z = maxHeight; z > minHeight; z -= 2) {
            Debug.Log($"generated ore at {z}");
            for (int largeX = 0; largeX < config.areaSize; largeX += 50) {
                for (int largeY = 0; largeY < config.areaSize; largeY += 50) {
                    string material = selectOreMaterial(z);
                    if (material == "none") continue;
                    int oreId = MaterialMap.get().id(material);
                    generator.createVein(50, oreArray);
                    for (int x = 0; x < 50; x++) {
                        for (int y = 0; y < 50; y++) {
                            if (oreArray[x, y] != 0) {
                                // int noiseZ = z - 1 + (int)Math.Floor(Mathf.PerlinNoise(xOffset + x * 0.03f, yOffset + y * 0.03f) * 2); // noised height
                                container.map.blockType.setRaw(largeX + x, largeY + y, z, BlockTypes.WALL.CODE, oreId);
                                oreArray[x, y] = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    private string selectOreMaterial(int z) {
        List<string> ores = oreDepthRanges.Where(pair => pair.Value.Item1 < z && pair.Value.Item2 > z)
            .Select(pair => pair.Key)
            .ToList();
        if (ores.Count == 0) return "none";
        return ores[UnityEngine.Random.Range(0, ores.Count)];
    }
}
}