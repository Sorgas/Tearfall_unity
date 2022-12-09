using game.model;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;

public class UnitVisualMovementSystem : LocalModelEcsSystem {
    // public EcsFilter<UnitMovementPathComponent, UnitMovementComponent> toAddFilter;

    public UnitVisualMovementSystem(LocalModel model) : base(model) { }

    public override void Run() {
        // currentSpeed = GameModel.get().updateController.speed;
        // foreach (int i in toAddFilter) {
        //     addVisualMovementComponent(toAddFilter.GetEntity(i), toAddFilter.Get2(i), toAddFilter.Get1(i));
        // }
    }

    private void addVisualMovementComponent(EcsEntity unit, UnitMovementComponent movement, UnitMovementPathComponent path) {
        // Vector3Int targetModel = path.path[0];
        // bool isOnRamp = GameModel.get().currentLocalModel.localMap.blockType.get(targetModel) == BlockTypes.RAMP.CODE
        //                     || unit.Has<UnitVisualOnBuildingComponent>(); // TODO or over building
        // Vector3 target = ViewUtil.fromModelToSceneForUnit(targetModel, model);
        // unit.Replace(new UnitVisualMovementComponent {
        //     target = target,
        //     spriteSpeed =
        //             (target - unit.take<UnitVisualComponent>().handler.gameObject.transform.localPosition).magnitude // step length
        //             * movement.speed * 30
        // });
    }
}