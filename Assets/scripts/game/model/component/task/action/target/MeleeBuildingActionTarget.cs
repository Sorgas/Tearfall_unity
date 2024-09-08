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
// ActionTarget to target building in melee combat
// public class MeleeBuildingActionTarget : BuildingActionTarget {
//
//     // public MeleeBuildingActionTarget(EcsEntity unit) : base(unit, ActionTargetTypeEnum.NEAR) { }
//     // public MeleeBuildingActionTarget(ActionTargetTypeEnum type) : base(type) { }
//     public MeleeBuildingActionTarget(EcsEntity entity) : base(entity, ActionTargetTypeEnum.NEAR) {
//         init();
//     }
//     
//     public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
//         Vector3Int pos = building.pos();
//         return PositionUtil.allNeighbour
//             .Select(offset => offset + pos)
//             .Where(position => model.localMap.inMap(position))
//             .Where(position => model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE)
//             .ToList();
//     }
// }
}