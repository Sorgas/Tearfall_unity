using System.Collections.Generic;
using System.Linq;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.geometry;
using util.lang.extension;

namespace game.model.component.task.action.target {
// ActionTarget for targeting unit in melee combat
public class MeleeUnitActionTarget : EntityActionTarget {
    // private EcsEntity unit
    
    public MeleeUnitActionTarget(EcsEntity unit) : base(unit, ActionTargetTypeEnum.NEAR) { }
    
    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        Vector3Int pos = entity.pos();
        return PositionUtil.allNeighbour
            .Select(offset => offset + pos)
            .Where(position => model.localMap.inMap(position))
            .Where(position => model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE)
            .ToList();
    }
}
}