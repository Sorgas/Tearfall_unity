using System.Collections.Generic;
using types.action;
using UnityEngine;

namespace game.model.component.task.action.target {
// points to immovable target, have precalculated values
public abstract class StaticActionTarget : ActionTarget {
    public override Vector3Int pos => staticPosition;
    protected override List<Vector3Int> positions => staticPositions;
    protected override List<Vector3Int> lowerPositions => staticLowerPositions;
    protected override List<Vector3Int> upperPositions => staticUpperPositions;
    // precalculated values
    private Vector3Int staticPosition;
    private List<Vector3Int> staticPositions;
    private List<Vector3Int> staticLowerPositions;
    private List<Vector3Int> staticUpperPositions;
    
    protected StaticActionTarget(ActionTargetTypeEnum type) : base(type) { }

    protected abstract Vector3Int calculatePosition();
    protected abstract List<Vector3Int> calculatePositions();
    
    // different z-level access is rare case
    protected virtual List<Vector3Int> calculateLowerPositions() => emptyList;
    protected virtual List<Vector3Int> calculateUpperPositions() => emptyList;

    public override void init() {
        staticPosition = calculatePosition();
        staticPositions = calculatePositions();
        staticLowerPositions = calculateLowerPositions();
        staticUpperPositions = calculateUpperPositions();
    }
}
}