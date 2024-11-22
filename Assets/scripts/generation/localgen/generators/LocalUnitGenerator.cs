using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.unit;
using game.model.localmap;
using generation.unit;
using Leopotam.Ecs;
using types;
using types.unit;
using types.unit.skill;
using UnityEngine;
using util.lang.extension;

namespace generation.localgen.generators {
// generates units on local map    
public class LocalUnitGenerator : LocalGenerator {
        private LocalMap map;
        private int spawnSearchMaxAttempts = 100;
        private UnitGenerator unitGenerator;

        public LocalUnitGenerator(LocalMapGenerator generator) : base(generator) {
            name = "LocalUnitGenerator";
        }

        protected override void generateInternal() {
            map = container.map;
            unitGenerator = new UnitGenerator(random(0, 1000));
            spawnUnits(GenerationState.get().preparationState.units);
        }

        private void spawnUnits(List<UnitData> settlers) {
            Vector2Int center = new(map.bounds.maxX / 2, map.bounds.maxY / 2);
            settlers.ForEach(settler => {
                Vector3Int? spawnPoint = getSpawnPosition(center, 5);
                if (spawnPoint.HasValue) {
                    EcsEntity entity = container.model.createEntity();
                    unitGenerator.generateUnit(settler, entity);
                    ref PositionComponent positionComponent = ref entity.takeRef<PositionComponent>();
                    positionComponent.position = spawnPoint.Value;
                    initJobs(entity);
                    container.model.unitContainer.statusEffectUtility.recalculate(entity);
                    container.model.factionContainer.addUnit(entity);
                    log("unit spawned at " + spawnPoint.Value);
                } else {
                    Debug.LogWarning("position to spawn unit not found");
                }
            });
        }

        private Vector3Int? getSpawnPosition(Vector2Int center, int range) {
            Vector3Int spawnPoint = new(center.x + random(-range, +range), center.y + random(-range, range), 0);
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

        // applies job assign policy to unit's skills
        private void initJobs(EcsEntity entity) {
            UnitJobsComponent jobsComponent = entity.take<UnitJobsComponent>();
            switch (GlobalSettings.jobAssignPolicy) {
                case 0: {
                    foreach (var job in Jobs.jobs) {
                        jobsComponent.enabledJobs[job.name] = 2;
                    }
                }
                break;
                case 1: {
                    IEnumerable<UnitSkill> skills = entity.take<UnitSkillComponent>().skills.Values.Where(skill => skill.level > 0 && Jobs.jobsBySkill.ContainsKey(skill.type.name));
                    foreach (var unitSkill in skills) {
                        jobsComponent.enabledJobs[Jobs.jobsBySkill[unitSkill.type.name].name] = 2;
                    }
                    jobsComponent.enabledJobs[Jobs.HAULER.name] = 2;
                    jobsComponent.enabledJobs[Jobs.BUILDER.name] = 2;
                }
                break;
                case 2: {
                    jobsComponent.enabledJobs[Jobs.HAULER.name] = 2;
                    jobsComponent.enabledJobs[Jobs.BUILDER.name] = 2;
                }
                break;
            }
        }
        
        public override string getMessage() {
            return "generating units..";
        }
    }
}