using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.geometry;
using util.lang.extension;

namespace game.model.component.task.action.target {
public abstract class ActionTarget {
    protected readonly ActionTargetTypeEnum targetType;
    public ActionTargetTypeEnum type => targetType;
    protected readonly List<Vector3Int> emptyList = new();
    
    // should return current action target position or Vector3Int.back
    // Vector3Int.back means target is not available
    // used to calculate proximity to target
    public virtual Vector3Int pos => Vector3Int.back;

    protected ActionTarget(ActionTargetTypeEnum type) {
        targetType = type;
    }
    
    // checks if performer reached action target
    public virtual string check(EcsEntity performer, LocalModel model) {
        List<Vector3Int> acceptablePositions = getAcceptablePositions(model);
        if (acceptablePositions.Count == 0) return "no positions"; // fail
        return acceptablePositions.Contains(performer.pos()) ? "ready" : "move";
    }

    // should return all positions filtered by all applicable conditions except accessibility area
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