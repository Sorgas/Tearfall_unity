using System.Collections.Generic;
using Assets.scripts.enums;
using Assets.scripts.generation;
using Assets.scripts.generation.localgen;
using Assets.scripts.util.geometry;
using Tearfall_unity.Assets.scripts.enums.material;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.generation.localgen.generators {

    // generates several layers of stone and soil materials and flushes them to map from surface to 
    public class LocalStoneLayersGenerator : LocalGenerator {
        private LocalGenContainer container;
        private LocalGenConfig config;
        private IntBounds2 bounds = new IntBounds2();
        private List<LayerDescriptor> layers = new List<LayerDescriptor>();
        private ValueRange layerThicknessRange = new ValueRange();
        private int[,] currentHeight;
        private float xOffset;
        private float yOffset;

        public override void generate() {
            config = GenerationState.get().localGenConfig;
            container = GenerationState.get().localGenContainer;
            currentHeight = new int[config.areaSize, config.areaSize];
            for (int x = 0; x < config.areaSize; x++) {
                for (int y = 0; y < config.areaSize; y++) {
                    currentHeight[x, y] = (int) container.heightsMap[x, y];
                }
            }
            generateLayers();
            layers.ForEach(layer => {
                fillLayer(layer);
            });
        }

        private void generateLayers() {
            layers.Add(generateLayer("soil", config.soilThickness - 1, config.soilThickness + 1));
            // sedimentary
            List<Material_> sedimentary = MaterialMap.get().getByTag("stone_sedimentary");
            sedimentary.ForEach(material => {
                LayerDescriptor layer = generateLayer(material.name, 4, 6);
                layers.Add(layer);
            });
            // metamorfic
            List<Material_> metamorfic = MaterialMap.get().getByTag("stone_metamorfic");
            sedimentary.ForEach(material => {
                LayerDescriptor layer = generateLayer(material.name, 4, 6);
                layers.Add(layer);
            });
            // igneous
            List<Material_> igneous = MaterialMap.get().getByTag("stone_igneous");
            sedimentary.ForEach(material => {
                LayerDescriptor layer = generateLayer(material.name, 4, 6);
                layers.Add(layer);
            });
            Debug.Log(layers.Count + " layers generated");
        }

        // adds layers blocks beneath currentHeight values
        private void fillLayer(LayerDescriptor layer) {
            container = GenerationState.get().localGenContainer;
            for (int x = 0; x < config.areaSize; x++) {
                for (int y = 0; y < config.areaSize; y++) {
                    if (currentHeight[x, y] <= 0) break;
                    for (int z = 0; z <= layer.layer[x, y]; z++) {
                        if (currentHeight[x, y] >= 0) {
                            container.localMap.blockType.setRaw(x, y, currentHeight[x, y], BlockTypeEnum.WALL.CODE, layer.material);
                            currentHeight[x, y]--;
                        }
                    }
                }
            }
        }

        private LayerDescriptor generateLayer(string material, int minThickness, int maxThickness) {
            LayerDescriptor layer = new LayerDescriptor(material, config.areaSize);
            layer.material = material;
            xOffset = Random.value * 10000;
            yOffset = Random.value * 10000;
            bounds.iterate((x, y) => layer.layer[x, y] += (int)Mathf.PerlinNoise(xOffset + x * 0.05f, yOffset + y * 0.05f));
            return layer;
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