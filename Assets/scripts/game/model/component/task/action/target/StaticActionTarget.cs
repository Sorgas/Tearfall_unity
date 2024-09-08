using System.Collections.Generic;
using types.action;
using UnityEngine;

namespace game.model.component.task.action.target {
// points to immovable target, have precalculated values
// precalculation methods should return not filtered positions
public abstract class StaticActionTarget : ActionTarget {
    public override Vector3Int pos => staticPos;
    protected Vector3Int staticPos;

    // all valid exact positions on same z-level (around center for NEAR). on pathfinding they are filtered to PASSABLE
    protected List<Vector3Int> positions;
    // all valid exact positions 1 z-level lower. on pathfinding, they are filtered to RAMP only
    protected List<Vector3Int> lowerPositions;
    // all valid exact positions 1 z-level higher. on pathfinding, they are filtered to RAMP only
    protected List<Vector3Int> upperPositions;

    protected StaticActionTarget(ActionTargetTypeEnum type) : base(type) { }

    protected abstract Vector3Int calculateStaticPosition();
    protected abstract List<Vector3Int> calculateStaticPositions();
    
    // different z-level access is rare case
    protected virtual List<Vector3Int> calculateLowerPositions() => emptyList;
    protected virtual List<Vector3Int> calculateUpperPositions() => emptyList;
    
    // calculates positions, should be called in subclasses constructors
    protected void init() {
        Debug.Log($"initing static action target {ToString()}");
        staticPos = calculateStaticPosition();
        positions = calculateStaticPositions();
        lowerPositions = calculateLowerPositions();
        upperPositions = calculateUpperPositions();
    }
}
}