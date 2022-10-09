using System.Collections.Generic;
using game.model.localmap;
using generation.item;
using Leopotam.Ecs;
using types;
using UnityEngine;

namespace generation.localgen.generators {
    public class LocalItemGenerator : LocalGenerator {
        private LocalMap map;
        private ItemGenerator generator = new();

        public LocalItemGenerator(LocalMapGenerator generator) : base(generator) { }

        public override void generate() {
            Debug.Log("generating local items");
            map = container.map;
            spawnItems(GenerationState.get().preparationState.items);
        }

        private void spawnItems(List<ItemData> items) {
            Vector2Int center = new Vector2Int(map.bounds.maxX / 2, map.bounds.maxY / 2);
            items.ForEach(item => {
                Vector3Int spawnPoint = getSpawnPosition(center, 5);
                for (int i = 0; i < item.quantity; i++) {
                    spawnItem(spawnPoint, item);
                }
            });
        }

        private void spawnItem(Vector3Int spawnPoint, ItemData item) {
            EcsEntity entity = container.model.createEntity();
            generator.generateItem(item.type, item.material, entity);
            container.model.itemContainer.onMap.putItemToMap(entity, spawnPoint);
        }

        private Vector3Int getSpawnPosition(Vector2Int center, int range) {
            Vector3Int spawnPoint = new Vector3Int(center.x + Random.Range(-range, +range), center.y + Random.Range(-range, range), 0);
            for (int z = map.bounds.maxZ - 1; z >= 0; z--) {
                int blockType = map.blockType.get(spawnPoint.x, spawnPoint.y, z);
                if (blockType == BlockTypes.FLOOR.CODE || blockType == BlockTypes.RAMP.CODE) {
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