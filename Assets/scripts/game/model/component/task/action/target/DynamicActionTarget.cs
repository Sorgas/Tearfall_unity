using System.Collections.Generic;
using types.action;
using UnityEngine;

namespace game.model.component.task.action.target {
// for targets, which position can change during action performing
public abstract class DynamicActionTarget : ActionTarget {
    protected DynamicActionTarget(ActionTargetTypeEnum type) : base(type) { }
    protected readonly List<Vector3Int> emptyList = new();
    protected override List<Vector3Int> positions => emptyList;
    protected override List<Vector3Int> lowerPositions => emptyList;

    public override void init() { }
}
}