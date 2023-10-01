using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util;
using util.geometry;
using util.lang.extension;
using static game.model.component.task.action.target.ActionTargetStatusEnum;
using static types.action.ActionTargetTypeEnum;

namespace game.model.component.task.action.target {
public abstract class ActionTarget {
    protected readonly ActionTargetTypeEnum targetType;
    public ActionTargetTypeEnum type => targetType;
    protected readonly List<Vector3Int> emptyList = new();
    
    // should return current action target position or Vector3Int.back
    // used to calculate proximity to target
    public virtual Vector3Int pos => Vector3Int.back;

    // should return all valid exact positions on same z-level (around center for NEAR). on pathfinding they are filtered to PASSABLE
    protected virtual List<Vector3Int> positions => emptyList;
    // should return all valid exact positions 1 z-level lower. on pathfinding, they are filtered to RAMP only
    protected virtual List<Vector3Int> lowerPositions => emptyList;
    // should return all valid exact positions 1 z-level higher. on pathfinding, they are filtered to RAMP only
    protected virtual List<Vector3Int> upperPositions => emptyList;
    
    protected ActionTarget(ActionTargetTypeEnum type) {
        targetType = type;
    }

    public abstract void init();
    
    // checks if performer reached action target
    public virtual string check(EcsEntity performer, LocalModel model) {
        List<Vector3Int> acceptablePositions = getAcceptablePositions(model);
        if (acceptablePositions.Count == 0) return "no positions"; // fail
        return getAcceptablePositions(model).Contains(performer.pos()) ? "ready" : "move";
    }

    // should filter all positions by passage
    public abstract List<Vector3Int> getAcceptablePositions(LocalModel model);

    private int getDistance(Vector3Int current, Vector3Int target, LocalModel model) {
        if (current == target) return 0;
        if (!current.isNeighbour(target)) return 2;
        if (current.z == target.z) return 1;
        if (current.z < target.z && model.localMap.blockType.get(current) == BlockTypes.RAMP.CODE) return 1;
        return 2;
    }
}
}