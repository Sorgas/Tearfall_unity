using System.Collections.Generic;
using enums.unit;
using game.model;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.unit {
    class UnitGenerator {
        // private BodyAspectGenerator bodyAspectGenerator = new BodyAspectGenerator();
        // private NeedAspectGenerator needAspectGenerator = new NeedAspectGenerator();
        // private HealthAspectGenerator healthAspectGenerator = new HealthAspectGenerator();
        // private HumanoidRenderGenerator humanoidRenderGenerator = new HumanoidRenderGenerator();
        // private JobSkillAspectGenerator jobSkillAspectGenerator = new JobSkillAspectGenerator();

        // public EcsEntity generateUnit(Vector3Int position, string specimen) {
        //     // Logger.GENERATION.log("generating unit " + specimen);
        //     return Optional.ofNullable(CreatureTypeMap.getType(specimen))
        //         .map(type-> {
        //         Unit unit = new Unit(position.clone(), type);
        //         addMandatoryAspects(unit);
        //         addOptionalAspects(unit);
        //         addRenderAspect(unit);
        //         return unit;
        //     })
        //     .orElse(null);
        // }

        // private void addMandatoryAspects(Unit unit) {
        //     CreatureType type = unit.getType();
        //     unit.add(needAspectGenerator.generateNeedAspect(type));
        //     unit.add(bodyAspectGenerator.generateBodyAspect(type));
        //     unit.add(healthAspectGenerator.generateHealthAspect(unit));
        //     unit.add(new TaskAspect(null));
        //     unit.add(new MovementAspect(null));
        //     unit.add(new MoodAspect());
        // }
        //
        // private void addOptionalAspects(Unit unit) {
        //     CreatureType type = unit.getType();
        //     for (String aspect :
        //     type.aspects) {
        //         switch (aspect) {
        //             case "equipment": {
        //                 unit.add(equipmentAspectGenerator.generateEquipmentAspect(type));
        //                 continue;
        //             }
        //             case "jobs": {
        //                 unit.add(jobSkillAspectGenerator.generate());
        //             }
        //         }
        //     }
        // }
        //
        // private void addRenderAspect(Unit unit) {
        //     CreatureType type = unit.getType();
        //     unit.add(type.combinedAppearance != null
        //         ? humanoidRenderGenerator.generateRender(type, true)
        //         : new RenderAspect(AtlasesEnum.units.getBlockTile(type.atlasXY)));
        // }


        private UnitEquipmentComponentGenerator equipmentGenerator = new UnitEquipmentComponentGenerator();

        public void generateUnit(SettlerData data, EcsEntity entity) {
            CreatureType type = CreatureTypeMap.getType(data.type);
            addCommonComponents(ref entity, data);
            addOptionalComponents(ref entity, type);
            addSettlerComponents(ref entity);
        }

        public void generateUnit(SettlerData data) => generateUnit(data, GameModel.get().createEntity());

        // TODO
        public void generateUnit(string creatureType) {
            CreatureType type = CreatureTypeMap.getType(creatureType);
            type.aspects.Contains("equipment");

        }

        private void addCommonComponents(ref EcsEntity entity, SettlerData data) {
            entity.Replace(new UnitNameComponent { name = "qwer" }) // TODO add name generator
                .Replace(new AgeComponent { age = 20 }) // TODO add name generator
                .Replace(new UnitMovementComponent { speed = 0.06f, step = 0 })
                .Replace(new UnitVisualComponent())
                .Replace(new NameComponent { name = "mockName" })
                .Replace(new PositionComponent { position = new Vector3Int() })
                .Replace(new UnitComponent());
        }

        private void addSettlerComponents(ref EcsEntity entity) {
            entity.Replace(new UnitJobsComponent { enabledJobs = new List<string>() });
        }

        private void addOptionalComponents(ref EcsEntity entity, CreatureType type) {
            if (type.aspects.Contains("equipment")) {
                entity.Replace(equipmentGenerator.generate(type));
            }
        }
    }
}