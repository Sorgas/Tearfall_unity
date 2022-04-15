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
        private UnitNeedComponentGenerator needGenerator = new UnitNeedComponentGenerator();

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
            needGenerator.generate(ref entity, type);
        }

        private void addSettlerComponents(ref EcsEntity entity) {
            entity.Replace(new UnitJobsComponent {enabledJobs = new List<string>()});
        }

        private void addOptionalComponents(ref EcsEntity entity, CreatureType type) {
            if (type.aspects.Contains("equipment")) {
                entity.Replace(equipmentGenerator.generate(type));
            }
        }
    }
}