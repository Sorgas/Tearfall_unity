using game.model.localmap;
using types;
using UnityEngine;

namespace generation.localgen.generators {
    public class LocalSubstrateGenerator : LocalGenerator {
        public LocalSubstrateGenerator(LocalMapGenerator generator) : base(generator) { }

        protected override void generateInternal() {
            LocalMap map = container.map;
            int mapHeight = map.bounds.maxZ;
            for (int x = 0; x <= map.bounds.maxX; x++) {
                for (int y = 0; y <= map.bounds.maxY; y++) {
                    for (int z = map.bounds.maxZ; z >= 0; z--) {
                        int blockType = map.blockType.get(x, y, z);
                        if (blockType == BlockTypes.FLOOR.CODE || blockType == BlockTypes.RAMP.CODE) {
                            // put substrate to tiles
                            // TODO add substrateType selection
                            Vector3Int position = new(x, y, z);
                            container.map.substrateMap.add(position, 1, true);
                            break;
                        }
                    }
                }
            }
        }

        public override string getMessage() {
            return "generating grass ...";
        }
    }
}