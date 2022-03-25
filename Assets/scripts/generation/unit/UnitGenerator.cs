using System.Collections.Generic;
using enums.unit;
using game.model;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.unit {
    class UnitGenerator {
        private UnitEquipmentComponentGenerator equipmentGenerator = new UnitEquipmentComponentGenerator();
        private UnitBodyComponentGenerator bodyGenerator = new UnitBodyComponentGenerator();
        private UnitNameGenerator nameGenerator = new UnitNameGenerator();

        public void generateUnit(SettlerData data, EcsEntity entity) {
            CreatureType type = CreatureTypeMap.getType(data.type);
            addCommonComponents(ref entity, data, type);
            addOptionalComponents(ref entity, type);
            addSettlerComponents(ref entity);
        }

        public void generateUnit(SettlerData data) => generateUnit(data, GameModel.get().createEntity());

        // TODO
        public void generateUnit(string creatureType) {
            CreatureType type = CreatureTypeMap.getType(creatureType);
            type.aspects.Contains("equipment");
        }

        private void addCommonComponents(ref EcsEntity entity, SettlerData data, CreatureType type) {
            // TODO add name generator
            entity.Replace(new UnitNameComponent {name = "qwer"}) 
                .Replace(new AgeComponent {age = 20})
                .Replace(new UnitMovementComponent {speed = 0.06f, step = 0})
                .Replace(new UnitVisualComponent())
                .Replace(nameGenerator.generate())
                .Replace(new PositionComponent {position = new Vector3Int()})
                .Replace(bodyGenerator.generate(type))
                .Replace(new UnitComponent());
        }

        private void addSettlerComponents(ref EcsEntity entity) {
            entity.Replace(new UnitJobsComponent {enabledJobs = new List<string>()});
        }

        private void addOptionalComponents(ref EcsEntity entity, CreatureType type) {
            if (type.aspects.Contains("equipment")) {
                entity.Replace(equipmentGenerator.generate(type));
            }
        }
        // private NeedAspectGenerator needAspectGenerator = new NeedAspectGenerator();
        // private HealthAspectGenerator healthAspectGenerator = new HealthAspectGenerator();
        // private HumanoidRenderGenerator humanoidRenderGenerator = new HumanoidRenderGenerator();

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
    }
}