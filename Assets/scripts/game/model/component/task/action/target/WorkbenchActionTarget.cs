using System.Collections.Generic;
using game.model.component.building;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
// targets to access position of workbench
class WorkbenchActionTarget : StaticActionTarget {
    private EcsEntity workbench;
    public WorkbenchActionTarget(EcsEntity entity) : base(ActionTargetTypeEnum.EXACT) {
        workbench = entity;
        init();
    }

    protected override Vector3Int calculateStaticPosition() {
        return workbench.take<BuildingComponent>().getAccessPosition(workbench.pos());
    }
    
    protected override List<Vector3Int> calculateStaticPositions() {
        return emptyList;
    }

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        return new List<Vector3Int> { pos };
    }
}
}