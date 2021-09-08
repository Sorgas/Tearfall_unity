using System.Collections.Generic;
using Assets.scripts.enums;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation;
using Assets.scripts.generation.localgen;
using Leopotam.Ecs;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.generation.localgen.generators {
    public class LocalUnitGenerator : LocalGenerator {
        private LocalMap map;
        private int spawnSearchMaxAttempts = 100;
        private UnitGenerator unitGenerator;

        public override void generate() {
            map = GenerationState.get().localGenContainer.localMap;
            spawnSettlers(GenerationState.get().preparationState.settlers);
        }

        private void spawnSettlers(List<SettlerData> settlers) {
            Vector2Int center = new Vector2Int(map.xSize / 2, map.ySize / 2);
            settlers.ForEach(settler => {
                Vector3Int spawnPoint = getSpawnPosition(center, 5);
                EcsEntity entity = GenerationState.get().ecsWorld.NewEntity();
                unitGenerator.generateToEntity(entity, settler);
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
    }
}