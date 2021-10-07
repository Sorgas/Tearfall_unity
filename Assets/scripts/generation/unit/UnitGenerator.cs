using System.Collections.Generic;
using game.model.component.unit.components;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.unit {
    class UnitGenerator {
        public void generateToEntity(EcsEntity entity, SettlerData data) {
            entity.Replace<UnitNameComponent>(new UnitNameComponent() { name = data.name }) // TODO add name generator
                .Replace<AgeComponent>(new AgeComponent() { age = data.age }) // TODO add name generator
                .Replace<MovementComponent>(new MovementComponent() { position = new Vector3Int(), path = new List<Vector3Int>(), speed = 0.06f, step = 0 })
                .Replace<UnitVisualComponent>(new UnitVisualComponent() { })
                .Replace<UnitComponent>(new UnitComponent() { });
        }

        // public void generateUnit(string specimen, Vector3Int position) {
        //     EcsEntity unitEntity = GameModel.get().ecsWorld.NewEntity();
        //     string qwer;
        //     TestComponent test = new TestComponent();
        //     test.value = "test Value 123";
        //     unitEntity.Replace(new PositionComponent() { position = new Vector3Int(0, 0, 0) })
        //     .Replace(test);
        // }
    }
}