using Assets.scripts.enums;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation;
using Assets.scripts.generation.localgen;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.generation.localgen.generators {
    public class LocalUnitGenerator : LocalGenerator {
        private LocalMap map;
        private int spawnSearchMaxAttempts = 100;
        private UnitGenerator unitGenerator;

        public override void generate() {
            map = GenerationState.get().localGenContainer.localMap;
            Vector3Int spawn = selectSpawnPosition();
        }

        // selects flat area on map without lakes to spawn settlers on
        private Vector3Int selectSpawnPosition() {
            int attempt = 0;
            while (attempt < spawnSearchMaxAttempts) {
                Vector3Int position = getSurfaceZ(map.xSize / 2 + Random.Range(-10, 10), map.ySize / 2 + Random.Range(-10, 10));
                if (position.z != -1) {
                    return position;
                }
            }
            return new Vector3Int(-1, -1, -1); // TODO fail localgen
        }

        private Vector3Int getSurfaceZ(int x, int y) {
            Vector3Int position = new Vector3Int(x, y, -1);
            for (int z = map.zSize - 1; z >= 0; z--) {
                int blockType = map.blockType.get(x, y, z);
                if (blockType == BlockTypeEnum.FLOOR.CODE || blockType == BlockTypeEnum.RAMP.CODE) {
                    position.z = z;
                    break;
                }
            }
            return position; // not found if -1
        }
    }
}