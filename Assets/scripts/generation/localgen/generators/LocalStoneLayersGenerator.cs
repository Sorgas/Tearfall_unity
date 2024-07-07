using System;
using System.Collections.Generic;
using types;
using types.material;
using UnityEngine;
using util.geometry;

namespace generation.localgen.generators {
// generates several layers of stone and soil materials and flushes them to local map 
public class LocalStoneLayersGenerator : LocalGenerator {
    private ValueRange layerThicknessRange = new();
    private int[,] currentHeight;
    private float xOffset;
    private float yOffset;

    // private float effectiveElevation = 0;

    public LocalStoneLayersGenerator(LocalMapGenerator generator) : base(generator) {
        debug = true;
    }

    public override void generate() {
        currentHeight = new int[config.areaSize, config.areaSize];
        bounds.iterate((x, y) => currentHeight[x, y] = (int)container.heightsMap[x, y]);
        // countAverageElevation();
        generateLayers();
        container.stoneLayers.ForEach(layer => { fillLayer(layer); });
        finalizeBottom();
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
        int localElevation = container.localElevation;
        int soilLayer = (int)Math.Max(localElevation * config.soilThickness, 1);
        container.stoneLayers.Add(generateLayer("soil", soilLayer - 1, soilLayer + 1));
        int sedimentaryLayer = (int)(localElevation * (0.25 + UnityEngine.Random.Range(0, 0.1f)));
        int metamorficLayer = (int)(localElevation * (0.25 + UnityEngine.Random.Range(0, 0.1f)));
        int igneousLayer = (int)(localElevation - soilLayer - sedimentaryLayer - metamorficLayer);
        createLayerGroup("stone_sedimentary", sedimentaryLayer, "sedimentary");
        createLayerGroup("stone_metamorfic", metamorficLayer, "metamorfic");
        createLayerGroup("stone_igneous", igneousLayer, "igneous");
        log($"{container.stoneLayers.Count} layers generated");
    }

    // generates layers of all materials with tag
    private void createLayerGroup(string tag, int totalThickness, string type) {
        List<Material_> materials = MaterialMap.get().getByTag(tag);
        int singleLayerThickness = totalThickness / materials.Count;
        materials.ForEach(material => container.stoneLayers.Add(generateLayer(material.name, singleLayerThickness - 1, singleLayerThickness + 1)));
        log(type + " stone layers created, total: " + totalThickness + ", single: " + singleLayerThickness);
    }

    // creates layer descriptor with perlin noise between given range
    private LayerDescriptor generateLayer(string material, int minThickness, int maxThickness) {
        LayerDescriptor layer = new LayerDescriptor(material, config.areaSize);
        layer.material = material;
        xOffset = UnityEngine.Random.value * 10000;
        yOffset = UnityEngine.Random.value * 10000;
        int thicknessRange = maxThickness - minThickness;
        bounds.iterate((x, y) =>
            layer.thickness[x, y] = minThickness + (int)(Mathf.PerlinNoise(xOffset + x * 0.03f, yOffset + y * 0.03f) * thicknessRange));
        log($"stone layer generated: {material} {minThickness} {maxThickness}");
        return layer;
    }

    // adds layers blocks beneath currentHeight values
    private void fillLayer(LayerDescriptor layer) {
        bounds.iterate((x, y) => {
            if (currentHeight[x, y] > 0) {
                for (int z = 0; z <= layer.thickness[x, y]; z++) {
                    if (currentHeight[x, y] >= 0) {
                        container.map.blockType.setRaw(x, y, currentHeight[x, y], BlockTypes.WALL.CODE, layer.material);
                        currentHeight[x, y]--;
                    }
                }
            }
        });
    }

    // fills all free space in the map bottom with last layer material
    private void finalizeBottom() {
        string material = container.stoneLayers[^1].material;
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

    public class LayerDescriptor {
        public string material;
        public int[,] thickness; // thickness of a layer

        public LayerDescriptor(string material, int areaSize) {
            this.material = material;
            thickness = new int[areaSize, areaSize];
        }
    }

    
    
    // public void GenerateOreVeins(int numberOfVeins, int veinSize) {
    //     for (int i = 0; i < numberOfVeins; i++) {
    //         int startX = random.Next(width);
    //         int startY = random.Next(height);
    //         int startZ = random.Next(depth);
    //         GenerateVein(startX, startY, startZ, veinSize);
    //     }
    // }
    //
    // private void GenerateVein(int startX, int startY, int startZ, int veinSize) {
    //     for (int i = 0; i < veinSize; i++) {
    //         int thickness = random.Next(2, 6); // Thickness between 2 and 5
    //         List<(int x, int y, int z)> veinPoints = new List<(int x, int y, int z)>();
    //
    //         for (int t = 0; t < thickness; t++) {
    //             int x = startX + random.Next(-1, 2);
    //             int y = startY + random.Next(-1, 2);
    //             int z = startZ + random.Next(-1, 2);
    //
    //             if (IsValidCoordinate(x, y, z))
    //                 veinPoints.Add((x, y, z));
    //         }
    //
    //         foreach (var point in veinPoints) {
    //             if (IsValidCoordinate(point.x, point.y, point.z)) {
    //                 map[point.x, point.y, point.z] = 1; // Assuming 1 represents metal ore
    //                 startX = point.x;
    //                 startY = point.y;
    //                 startZ = point.z;
    //             }
    //         }
    //     }
    // }
    //
    // private bool IsValidCoordinate(int x, int y, int z) {
    //     return x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < depth;
    // }
    //
    // public void PrintMapSlice(int zLevel) {
    //     for (int y = 0; y < height; y++) {
    //         for (int x = 0; x < width; x++) {
    //             Console.Write(map[x, y, zLevel] + " ");
    //         }
    //         Console.WriteLine();
    //     }
    // }
}
}