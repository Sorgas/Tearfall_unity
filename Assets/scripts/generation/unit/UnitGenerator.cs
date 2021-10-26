using System.Collections.Generic;
using enums.unit;
using game.model.component;
using game.model.component.unit.components;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.unit {
    class UnitGenerator {
        public void generateToEntity(EcsEntity entity, SettlerData data) {
            entity.Replace(new UnitNameComponent { name = data.name }) // TODO add name generator
                .Replace(new AgeComponent { age = data.age }) // TODO add name generator
                .Replace(new MovementComponent { position = new Vector3Int(), path = new List<Vector3Int>(), speed = 0.06f, step = 0 })
                .Replace(new UnitVisualComponent())
                .Replace(new JobsComponent {enabledJobs = new List<string>()})
                .Replace(new NameComponent() {name = "mockName"})
                .Replace(new UnitComponent());
        }
    }
}