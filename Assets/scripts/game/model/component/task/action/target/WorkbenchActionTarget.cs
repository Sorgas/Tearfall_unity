using game.model.component.building;
using Leopotam.Ecs;
using types.action;
using types.building;
using UnityEngine;
using util.lang.extension;

// targets to access position of workbench
namespace game.model.component.task.action.target {
class WorkbenchActionTarget : EntityActionTarget {
    public WorkbenchActionTarget(EcsEntity entity) : base(entity, ActionTargetTypeEnum.EXACT) {
        pos = entity.take<BuildingComponent>().getAccessPosition(entity.pos());
    }

    public override Vector3Int pos { get; }
}
}