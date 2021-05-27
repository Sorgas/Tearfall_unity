using System.Collections.Generic;
using Assets.scripts.enums;
using Assets.scripts.generation;
using Assets.scripts.generation.localgen;
using Assets.scripts.util.geometry;

namespace Tearfall_unity.Assets.scripts.generation.localgen.generators {
    public class LocalStoneLayersGenerator : LocalGenerator {
        private LocalGenContainer container;
        private IntBounds2 bounds = new IntBounds2();
        private List<LayerDescriptor> layers = new List<LayerDescriptor>();
        private float xOffset;
        private float yOffset;

        public override void generate() {
            LocalGenConfig config = GenerationState.get().localGenConfig;
            container = GenerationState.get().localGenContainer;
            for (int x = 0; x < config.areaSize; x++) {
                for (int y = 0; y < config.areaSize; y++) {
                    for (int z = 0; z < container.heightsMap[x, y]; z++) {
                        container.localMap.blockType.setRaw(x, y, z, BlockTypeEnum.WALL.CODE, "granite");
                    }
                }
            }
        }

        private void generateLayers() {

        }

        private void generateLayer(string material) {
            LayerDescriptor layer = new LayerDescriptor();
            layer.material = material;
        }

        private class LayerDescriptor {
            public string material;
            public ValueRange thicknessRange;
            public int[][] layer;
        }
    }
}