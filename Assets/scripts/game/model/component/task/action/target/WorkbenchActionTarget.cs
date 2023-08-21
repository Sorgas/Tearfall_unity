using System.Collections.Generic;
using game.model.component.building;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
// targets to access position of workbench
class WorkbenchActionTarget : EntityActionTarget {
    public WorkbenchActionTarget(EcsEntity entity) : base(entity, ActionTargetTypeEnum.EXACT) {
        pos = entity.take<BuildingComponent>().getAccessPosition(entity.pos());
    }

    public override Vector3Int pos { get; }
    
    public override List<Vector3Int> getPositions(LocalModel model) {
        throw new System.NotImplementedException();
    }
}
}