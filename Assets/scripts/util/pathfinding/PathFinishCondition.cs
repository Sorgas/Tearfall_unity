using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.enums.action;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.extension;
using Assets.scripts.util.geometry;
using UnityEditor;
using UnityEditor.UIElements;
using static Assets.scripts.enums.action.ActionTargetTypeEnum;
using static Assets.scripts.enums.PassageEnum;
using static Assets.scripts.enums.BlockTypeEnum;

namespace Assets.scripts.util.pathfinding {
    class PathFinishCondition {
        // positions, where path can finish
        public readonly HashSet<IntVector3> acceptable = new HashSet<IntVector3>();

        public PathFinishCondition(IntVector3 target, ActionTargetTypeEnum targetType) {
            if (targetType == EXACT || targetType == ANY) acceptable.Add(target);
            if (targetType == NEAR || targetType == ANY) { // add near tiles
                LocalMap map = GameModel.get<LocalMap>(); // add near tiles
                PositionUtil.allNeighbour
                        .Select(delta => IntVector3.add(target, delta))
                        .Where(pos => map.inMap(pos))
                        .Where(pos => map.passageMap.passage.get(pos) == PASSABLE.VALUE)
                        .Apply(pos => acceptable.Add(pos));
                PositionUtil.allNeighbour
                        .Select(delta => IntVector3.add(target, delta))
                        .Select(pos => pos.add(0, 0, -1))
                        .Where(pos => map.inMap(pos))
                        .Where(pos => map.blockType.get(pos) == RAMP.CODE)
                        .Apply(pos => acceptable.Add(pos));
            }
            if (targetType != NEAR && targetType != ANY) return;
        }

        public bool check(IntVector3 current) {
            return acceptable.Contains(current);
        }
    }
}
