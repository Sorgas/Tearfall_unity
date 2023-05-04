using System.Collections.Generic;
using game.model.component;
using game.model.component.unit;
using game.model.localmap;
using generation.unit;
using Leopotam.Ecs;
using types;
using types.unit;
using UnityEngine;
using util.lang.extension;

namespace generation.localgen.generators {
    public class LocalUnitGenerator : LocalGenerator {
        private LocalMap map;
        private int spawnSearchMaxAttempts = 100;
        private UnitGenerator unitGenerator = new();

        public LocalUnitGenerator(LocalMapGenerator generator) : base(generator) { }

        public override void generate() {
            map = container.map;
            spawnSettlers(GenerationState.get().preparationState.settlers);
        }

        private void spawnSettlers(List<SettlerData> settlers) {
            Vector2Int center = new(map.bounds.maxX / 2, map.bounds.maxY / 2);
            settlers.ForEach(settler => {
                Vector3Int? spawnPoint = getSpawnPosition(center, 5);
                if (spawnPoint.HasValue) {
                    EcsEntity entity = container.model.createEntity();
                    unitGenerator.generateUnit(settler, entity);
                    ref PositionComponent positionComponent = ref entity.Get<PositionComponent>();
                    positionComponent.position = spawnPoint.Value;
                    ref UnitVisualComponent visual = ref entity.takeRef<UnitVisualComponent>();

                    // TODO move to settlerdata
                    entity.Get<UnitJobsComponent>().enabledJobs.Add(Jobs.NONE.name);
                    entity.Get<UnitJobsComponent>().enabledJobs.Add(Jobs.MINER.name);
                    entity.Get<UnitJobsComponent>().enabledJobs.Add(Jobs.WOODCUTTER.name);
                    entity.Get<UnitJobsComponent>().enabledJobs.Add(Jobs.BUILDER.name);
                    entity.Get<UnitJobsComponent>().enabledJobs.Add(Jobs.FARMER.name);

                    Debug.Log("unit spawned at " + spawnPoint.Value);
                } else {
                    Debug.LogWarning("position to spawn unit not found");
                }
            });
        }

        private Vector3Int? getSpawnPosition(Vector2Int center, int range) {
            Vector3Int spawnPoint = new(center.x + Random.Range(-range, +range), center.y + Random.Range(-range, range), 0);
            for (int z = map.bounds.maxZ - 1; z >= 0; z--) {
                int blockType = map.blockType.get(spawnPoint.x, spawnPoint.y, z);
                if (blockType == BlockTypes.FLOOR.CODE || blockType == BlockTypes.RAMP.CODE) {
                    spawnPoint.z = z;
                    return spawnPoint;
                }
            }
            Debug.LogWarning("spawn point not found");
            return null;
        }

        public override string getMessage() {
            return "generating units..";
        }
    }
}