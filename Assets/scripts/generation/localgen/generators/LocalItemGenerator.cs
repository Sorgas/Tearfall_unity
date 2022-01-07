using System.Collections.Generic;
using enums;
using game.model.localmap;
using generation.item;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.localgen.generators {
    public class LocalItemGenerator : LocalGenerator {
        private LocalMap map;
        private ItemGenerator generator = new ItemGenerator();
        
        public override void generate() {
            Debug.Log("generating local items");
            map = GenerationState.get().localGenContainer.localMap;
            spawnItems(GenerationState.get().preparationState.items);
        }

        private void spawnItems(List<ItemData> items) {
            Vector2Int center = new Vector2Int(map.xSize / 2, map.ySize / 2);
            items.ForEach(item => {
                Vector3Int spawnPoint = getSpawnPosition(center, 5);
                EcsEntity entity = GenerationState.get().ecsWorld.NewEntity();
                generator.generateItem(item, spawnPoint, entity);
            });
        }

        private Vector3Int getSpawnPosition(Vector2Int center, int range) {
            Vector3Int spawnPoint = new Vector3Int(center.x + Random.Range(-range, +range), center.y + Random.Range(-range, range), 0);
            for (int z = map.zSize - 1; z >= 0; z--) {
                int blockType = map.blockType.get(spawnPoint.x, spawnPoint.y, z);
                if (blockType == BlockTypeEnum.FLOOR.CODE || blockType == BlockTypeEnum.RAMP.CODE) {
                    spawnPoint.z = z;
                    break;
                }
            }
            return spawnPoint;
        }
        
        public override string getMessage() {
            return "";
        }
    }
}