using Assets.scripts.game.model;
using Leopotam.Ecs;
using UnityEngine;

class UnitGenerator {

    public void generateUnit(string specimen, Vector3Int position) {
        EcsEntity unitEntity = GameModel.get().ecsWorld.NewEntity();
        TestComponent test = new TestComponent();
        test.value = "test Value 123";
        unitEntity.Replace(new PositionComponent() { position = new Vector3Int(0, 0, 0) })
        .Replace(test);
    }
}