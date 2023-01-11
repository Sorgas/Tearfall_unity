using System.Collections.Generic;
using System.Linq;
using game.model.localmap;
using types.action;
using UnityEngine;
using util.geometry;
using static types.action.ActionTargetTypeEnum;
using static types.PassageTypes;
using static types.BlockTypes;

namespace util.pathfinding {
    class PathFinishCondition {
        // positions, where path can finish
        private readonly HashSet<Vector3Int> acceptable = new HashSet<Vector3Int>();
        private readonly ActionTargetTypeEnum targetType;

        public PathFinishCondition(Vector3Int target, ActionTargetTypeEnum targetType, LocalMap map) {
            this.targetType = targetType;
            if (targetType == EXACT || targetType == ANY) acceptable.Add(target);
            if (targetType == NEAR || targetType == ANY) { // add near tiles
                PositionUtil.allNeighbour
                        .Select(delta => target + delta)
                        .Where(pos => map.inMap(pos))
                        .Where(pos => map.passageMap.passage.get(pos) == PASSABLE.VALUE)
                        .ToList().ForEach(pos => acceptable.Add(pos));
                PositionUtil.allNeighbour
                        .Select(delta => target + delta)
                        .Select(pos => pos.add(0, 0, -1))
                        .Where(pos => map.inMap(pos))
                        .Where(pos => map.blockType.get(pos) == RAMP.CODE)
                        .ToList().ForEach(pos => acceptable.Add(pos));
            }
        }

        public bool check(Vector3Int current) => acceptable.Contains(current);

        public string getMessage() {
            string logString = targetType +  ": ";
            foreach (var pos in acceptable) {
                logString += pos + " ";
            }
            return logString;
        }
    } 
}
