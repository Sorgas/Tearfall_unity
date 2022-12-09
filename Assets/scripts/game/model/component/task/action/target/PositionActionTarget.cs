//Targets some tile.

using enums.action;
using UnityEngine;

namespace game.model.component.task.action.target {
    public class PositionActionTarget : ActionTarget {
        private Vector3Int targetPosition;

        public PositionActionTarget(Vector3Int targetPosition, ActionTargetTypeEnum placement) : base(placement) {
            this.targetPosition = targetPosition;
        }

        public override Vector3Int? Pos => targetPosition;
    }
}
