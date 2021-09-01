using System.Collections.Generic;
using Assets.scripts.enums;
using Assets.scripts.generation;
using Assets.scripts.generation.localgen;
using Assets.scripts.util.geometry;
using Tearfall_unity.Assets.scripts.enums.material;
using Unity.Mathematics;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.generation.localgen.generators {

    // generates several layers of stone and soil materials and flushes them to map from surface to 
    public class LocalStoneLayersGenerator : LocalGenerator {
        private List<LayerDescriptor> layers = new List<LayerDescriptor>();
        private ValueRange layerThicknessRange = new ValueRange();
        private int[,] currentHeight;
        private float xOffset;
        private float yOffset;

        private float averageElevation = 0;
        private float maxElevation = 0;

        public override void generate() {
            currentHeight = new int[config.areaSize, config.areaSize];
            bounds.iterate((x, y) => currentHeight[x, y] = (int)container.heightsMap[x, y]);
            countAverageElevation();
            generateLayers();
            layers.ForEach(layer => {
                fillLayer(layer);
            });
        }

        private void countAverageElevation() {
            float average = 0;
            maxElevation = 0;
            bounds.iterate((x, y) => {
                float elevation = container.heightsMap[x, y];
                average += elevation;
                if (maxElevation < elevation) maxElevation = elevation;
            });
            averageElevation /= (config.areaSize * config.areaSize);
        }

        private void generateLayers() {
            float mainElevation = averageElevation + (maxElevation - averageElevation) / 2;
            int soilLayer = (int)math.max((mainElevation * config.soilThickness), 1);
            int sedimentaryLayer = (int)(mainElevation * (0.25 + UnityEngine.Random.Range(0, 0.1f)));
            int metamorficLayer = (int)(mainElevation * (0.25 + UnityEngine.Random.Range(0, 0.1f)));
            int ignousLayer = (int)(mainElevation - soilLayer - sedimentaryLayer - metamorficLayer);

            List<Material_> sedimentary = MaterialMap.get().getByTag("stone_sedimentary");
            List<Material_> metamorfic = MaterialMap.get().getByTag("stone_metamorfic");
            List<Material_> igneous = MaterialMap.get().getByTag("stone_igneous");

            layers.Add(generateLayer("soil", soilLayer, soilLayer + 1));
            sedimentary.ForEach(material => {
                layers.Add(generateLayer(material.name, sedimentaryLayer / sedimentary.Count, ));
            });
            metamorfic.ForEach(material => {
                LayerDescriptor layer = generateLayer(material.name, 4, 6);
                layers.Add(layer);
            });
            igneous.ForEach(material => {
                LayerDescriptor layer = generateLayer(material.name, 4, 6);
                layers.Add(layer);
            });
            Debug.Log(layers.Count + " layers generated");
        }

        private void createLayerGroup(List<Material_> materials, int totalThickness) {
            int singleLayerThickness = totalThickness / materials
        }

        // adds layers blocks beneath currentHeight values
        private void fillLayer(LayerDescriptor layer) {
            container = GenerationState.get().localGenContainer;
            bounds.iterate((x, y) => {
                if (currentHeight[x, y] > 0) {
                    for (int z = 0; z <= layer.layer[x, y]; z++) {
                        if (currentHeight[x, y] >= 0) {
                            container.localMap.blockType.setRaw(x, y, currentHeight[x, y], BlockTypeEnum.WALL.CODE, layer.material);
                            currentHeight[x, y]--;
                        }
                    }
                }
            });
        }

        private LayerDescriptor generateLayer(string material, int minThickness, int maxThickness) {
            LayerDescriptor layer = new LayerDescriptor(material, config.areaSize);
            layer.material = material;
            xOffset = UnityEngine.Random.value * 10000;
            yOffset = UnityEngine.Random.value * 10000;
            bounds.iterate((x, y) => layer.layer[x, y] += (int)Mathf.PerlinNoise(xOffset + x * 0.05f, yOffset + y * 0.05f));
            return layer;
        }


        // soil sedimentary metamorfic igneous
        private int[] defineLayersSize() {

            List<Material_> sedimentary = MaterialMap.get().getByTag("stone_sedimentary");
            List<Material_> metamorfic = MaterialMap.get().getByTag("stone_metamorfic");
            List<Material_> igneous = MaterialMap.get().getByTag("stone_igneous");


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