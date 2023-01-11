using System.Collections.Generic;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit;
using UnityEngine;

namespace generation.unit {
    class UnitGenerator {
        private UnitEquipmentComponentGenerator equipmentGenerator = new();
        private UnitBodyComponentGenerator bodyGenerator = new();
        private UnitNameGenerator nameGenerator = new();
        private UnitNeedComponentGenerator needGenerator = new();

        public void generateUnit(SettlerData data, EcsEntity entity) {
            CreatureType type = CreatureTypeMap.getType(data.type);
            addCommonComponents(ref entity, data, type);
            addOptionalComponents(ref entity, type);
            addSettlerComponents(ref entity);
        }
        // TODO
        public void generateUnit(string creatureType) {
            CreatureType type = CreatureTypeMap.getType(creatureType);
            type.aspects.Contains("equipment");
        }

        private void addCommonComponents(ref EcsEntity entity, SettlerData data, CreatureType type) {
            entity.Replace(new AgeComponent {age = 20})
                .Replace(new UnitMovementComponent {speed = 0.03f, step = 0})
                .Replace(new UnitVisualComponent()) // sprite go is created in UnitVisualSystem
                .Replace(nameGenerator.generate(data))
                .Replace(new PositionComponent {position = new Vector3Int()})
                .Replace(bodyGenerator.generate(type))
                .Replace(new HealthComponent{overallStatus = "healthy"})
                .Replace(new MoodComponent {status = "content"})
                .Replace(new OwnershipComponent {wealthStatus = "poor"})
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