using System;
using System.Collections.Generic;
using enums;
using enums.material;
using game.model;
using game.model.localmap;
using types;
using types.material;
using UnityEngine;
using util.geometry;

namespace generation.localgen.generators {

    // generates several layers of stone and soil materials and flushes them to map from surface to 
    public class LocalStoneLayersGenerator : LocalGenerator {
        private List<LayerDescriptor> layers = new List<LayerDescriptor>();
        private ValueRange layerThicknessRange = new ValueRange();
        private LocalMap map;
        private int[,] currentHeight;
        private float xOffset;
        private float yOffset;

        private float effectiveElevation = 0;
        private float maxElevation = 0;

        public override void generate() {
            container = GenerationState.get().localGenContainer;
            map = GameModel.localMap;
            currentHeight = new int[config.areaSize, config.areaSize];
            bounds.iterate((x, y) => currentHeight[x, y] = (int)container.heightsMap[x, y]);
            countAverageElevation();
            generateLayers();
            layers.ForEach(layer => {
                fillLayer(layer);
            });
            finalizeBottom();
        }

        // counts average
        private void countAverageElevation() {
            float average = 0;
            maxElevation = 0;
            bounds.iterate((x, y) => {
                float elevation = container.heightsMap[x, y];
                average += elevation;
                if (maxElevation < elevation) maxElevation = elevation;
            });
            float averageElevation = average / (config.areaSize * config.areaSize);
            effectiveElevation = averageElevation + (maxElevation - averageElevation) / 2;
        }

        private void generateLayers() {
            int soilLayer = (int) Math.Max((effectiveElevation * config.soilThickness), 1);
            layers.Add(generateLayer("soil", soilLayer - 1, soilLayer + 1));
            int sedimentaryLayer = (int)(effectiveElevation * (0.25 + UnityEngine.Random.Range(0, 0.1f)));
            int metamorficLayer = (int)(effectiveElevation * (0.25 + UnityEngine.Random.Range(0, 0.1f)));
            int igneousLayer = (int)(effectiveElevation - soilLayer - sedimentaryLayer - metamorficLayer);
            createLayerGroup("stone_sedimentary", sedimentaryLayer, "sedimentary");
            createLayerGroup("stone_metamorfic", metamorficLayer, "metamorfic");
            createLayerGroup("stone_igneous", igneousLayer, "igneous");
            Debug.Log(layers.Count + " layers generated");
        }

        private void createLayerGroup(string tag, int totalThickness, string type) {
            List<Material_> materials = MaterialMap.get().getByTag(tag);
            int singleLayerThickness = totalThickness / materials.Count;
            materials.ForEach(material => layers.Add(generateLayer(material.name, singleLayerThickness - 1, singleLayerThickness + 1)));
            Debug.Log(type + " stone layers created, total: " + totalThickness + ", single: " + singleLayerThickness);
        }

        private LayerDescriptor generateLayer(string material, int minThickness, int maxThickness) {
            LayerDescriptor layer = new LayerDescriptor(material, config.areaSize);
            layer.material = material;
            xOffset = UnityEngine.Random.value * 10000;
            yOffset = UnityEngine.Random.value * 10000;
            int noiseMod = maxThickness - minThickness;
            bounds.iterate((x, y) => layer.layer[x, y] = minThickness + (int)Mathf.PerlinNoise(xOffset + x * 0.05f, yOffset + y * 0.05f) * noiseMod);
            return layer;
        }

        // adds layers blocks beneath currentHeight values
        private void fillLayer(LayerDescriptor layer) {
            bounds.iterate((x, y) => {
                if (currentHeight[x, y] > 0) {
                    for (int z = 0; z <= layer.layer[x, y]; z++) {
                        if (currentHeight[x, y] >= 0) {
                            map.blockType.setRaw(x, y, currentHeight[x, y], BlockTypes.WALL.CODE, layer.material);
                            currentHeight[x, y]--;
                        }
                    }
                }
            });
        }

        // fills all free space in the map bottom with last layer material
        private void finalizeBottom() {
            string material = layers[layers.Count - 1].material;
            bounds.iterate((x, y) => {
                if (currentHeight[x, y] >= 0) {
                    for(int z = 0; z <= currentHeight[x, y]; z++) {
                        map.blockType.setRaw(x, y, z, BlockTypes.WALL.CODE, material);
                    }
                }
            });
        }

        public override string getMessage() {
            return "generating stone layers..";
        }

        private class LayerDescriptor {
            public string material;
            public int[,] layer;

            public LayerDescriptor(string material, int areaSize) {
                this.material = material;
                layer = new int[areaSize, areaSize];
            }
        }
    }
}