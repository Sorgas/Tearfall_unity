using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;

namespace game.model.component.task.action.target {
// targets item lying on the ground
public class ItemActionTarget : DynamicActionTarget {
    public EcsEntity item;
    public override Vector3Int pos => item.pos();

    public ItemActionTarget(EcsEntity item) : base(EXACT) {
        this.item = item;
    }
    
    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        return new List<Vector3Int> {pos};
    }
}
}