
using types.action;
using UnityEngine;

// targets some tile.
namespace game.model.component.task.action.target {
    public class PositionActionTarget : ActionTarget {

        public PositionActionTarget(Vector3Int targetPosition, ActionTargetTypeEnum placement) : base(placement) {
            pos = targetPosition;
        }

        public override Vector3Int pos { get; }
    }
}
