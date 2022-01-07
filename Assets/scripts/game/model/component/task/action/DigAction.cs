using game.model.component.task.action.target;
using UnityEngine;
using static enums.action.ActionTargetTypeEnum;
using static game.model.component.task.DesignationComponents;

namespace game.model.component.task.action {
    public class DigAction : Action {

        public DigAction(DesignationComponent designation, PositionComponent position) : base(new PositionActionTarget(position.position, NEAR)) {
            Debug.Log("Dig action created for " + position.position);

        }
    }
}