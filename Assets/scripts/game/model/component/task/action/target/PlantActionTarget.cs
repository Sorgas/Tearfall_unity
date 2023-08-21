using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
// targets plant for harvesting, cutting or chopping
public class PlantActionTarget : EntityActionTarget {

    public PlantActionTarget(EcsEntity plant) : base(plant, ActionTargetTypeEnum.NEAR) {
    }
    
    public PlantActionTarget(EcsEntity plant, ActionTargetTypeEnum type) : base(plant, type) {
    }

    public override Vector3Int pos => entity.pos();
    
    public override List<Vector3Int> getPositions(LocalModel model) {
        throw new System.NotImplementedException();
    }
}
}