using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action.target;
using game.model.localmap;
using MoreLinq;
using types.action;
using UnityEngine;
using util.geometry;
using static types.action.ActionTargetTypeEnum;
using static types.PassageTypes;
using static types.BlockTypes;

namespace util.pathfinding {
class PathFinishCondition {
    public readonly ActionTargetTypeEnum targetType;
    public readonly Vector3Int main;
    // positions, where path can finish
    private readonly HashSet<Vector3Int> acceptable = new();

    public PathFinishCondition(ActionTarget target, LocalModel model) {
        target.getAcceptablePositions(model).ForEach(position => acceptable.Add(position));
        // LocalMap map = model.localMap;
        // main = target.pos;
        // if (target.type == EXACT || target.type == ANY) acceptable.Add(main);
        // if (target.type == NEAR || target.type == ANY) { // add near tiles
        //     target.positions
        //         .Where(map.inMap)
        //         .Where(pos => map.passageMap.passage.get(pos) == PASSABLE.VALUE)
        //         .ForEach(pos => acceptable.Add(pos));
        //     target.lowerPositions // targets are accessible from lower ramps
        //         .Where(map.inMap)
        //         .Where(pos => map.blockType.get(pos) == RAMP.CODE)
        //         .ForEach(pos => acceptable.Add(pos));
        // }
    }

    public PathFinishCondition(Vector3Int target, ActionTargetTypeEnum targetType, LocalModel model) {
        this.targetType = targetType;
        LocalMap map = model.localMap;
        if (targetType == EXACT || targetType == ANY) acceptable.Add(target);
        if (targetType == NEAR || targetType == ANY) { // add near tiles
            PositionUtil.allNeighbour
                .Select(delta => target + delta)
                .Where(pos => map.inMap(pos))
                .Where(pos => map.passageMap.passage.get(pos) == PASSABLE.VALUE)
                .ToList().ForEach(pos => acceptable.Add(pos));
            PositionUtil.allNeighbour // targets are accessible from lower ramps
                .Select(delta => target + delta)
                .Select(pos => pos.add(0, 0, -1))
                .Where(pos => map.inMap(pos))
                .Where(pos => map.blockType.get(pos) == RAMP.CODE)
                .ToList().ForEach(pos => acceptable.Add(pos));
        }
    }

    public bool check(Vector3Int current) => acceptable.Contains(current);

    public string getMessage() {
        string logString = targetType + ": ";
        foreach (var pos in acceptable) {
            logString += pos + " ";
        }
        return logString;
    }
}
}