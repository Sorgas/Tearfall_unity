using Leopotam.Ecs;
using Tearfall_unity.Assets.scripts.generation;
using UnityEngine;

class UnitGenerator {
    public void generateToEntity(EcsEntity entity, SettlerData data) {
        entity.Replace<UnitNameComponent>(new UnitNameComponent() { name = data.name }) // TODO add name generator
            .Replace<AgeComponent>(new AgeComponent() { age = data.age }) // TODO add name generator
            .Replace<MovementComponent>(new MovementComponent() {position = new Vector3Int(), speed = 6, step = 0, hasTarget = false })
            .Replace<VisualMovementComponent>(new VisualMovementComponent() { })
            .Replace<UnitVisualComponent>(new UnitVisualComponent() { });
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