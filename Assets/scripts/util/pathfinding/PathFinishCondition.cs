using System.Collections.Generic;
using System.Linq;
using enums.action;
using game.model;
using game.model.localmap;
using UnityEngine;
using util.extension;
using util.geometry;
using static enums.action.ActionTargetTypeEnum;
using static enums.PassageEnum;
using static enums.BlockTypeEnum;

namespace util.pathfinding {
    class PathFinishCondition {
        // positions, where path can finish
        private readonly HashSet<Vector3Int> acceptable = new HashSet<Vector3Int>();

        public PathFinishCondition(Vector3Int target, ActionTargetTypeEnum targetType) {
            if (targetType == EXACT || targetType == ANY) acceptable.Add(target);
            if (targetType == NEAR || targetType == ANY) { // add near tiles
                LocalMap map = GameModel.get().localMap;
                PositionUtil.allNeighbour
                        .Select(delta => target + delta)
                        .Where(pos => map.inMap(pos))
                        .Where(pos => map.passageMap.passage.get(pos) == PASSABLE.VALUE)
                        .Apply(pos => acceptable.Add(pos));
                PositionUtil.allNeighbour
                        .Select(delta => target + delta)
                        .Select(pos => pos.add(0, 0, -1))
                        .Where(pos => map.inMap(pos))
                        .Where(pos => map.blockType.get(pos) == RAMP.CODE)
                        .Apply(pos => acceptable.Add(pos));
            }
            if (targetType != NEAR && targetType != ANY) return;
        }

        public bool check(Vector3Int current) {
            return acceptable.Contains(current);
        }
    }
}
