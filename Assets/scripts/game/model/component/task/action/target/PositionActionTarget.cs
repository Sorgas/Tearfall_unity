//Targets some tile.
using Assets.scripts.enums.action;
using UnityEngine;

public class PositionActionTarget : ActionTarget {
    private Vector3Int targetPosition;

    public PositionActionTarget(Vector3Int targetPosition, ActionTargetTypeEnum placement) : base(placement) {
        this.targetPosition = targetPosition;
    }

    public override Vector3Int getPosition() {
        return targetPosition;
    }
}
