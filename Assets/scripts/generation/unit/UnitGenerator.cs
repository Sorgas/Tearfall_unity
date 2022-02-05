using System.Collections.Generic;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.unit {
    class UnitGenerator {
        private UnitEquipmentComponentGenerator equipmentGenerator = new UnitEquipmentComponentGenerator();
        
        public void generateToEntity(EcsEntity entity, SettlerData data) {
            entity.Replace(new UnitNameComponent { name = data.name }) // TODO add name generator
                .Replace(new AgeComponent { age = data.age }) // TODO add name generator
                .Replace(new UnitMovementComponent { speed = 0.06f, step = 0 })
                .Replace(new UnitVisualComponent())
                .Replace(new UnitJobsComponent {enabledJobs = new List<string>()})
                .Replace(new NameComponent {name = "mockName"})
                .Replace(new PositionComponent {position = new Vector3Int()})
                .Replace(equipmentGenerator.generate())
                .Replace(new UnitComponent());
        }
    }
}