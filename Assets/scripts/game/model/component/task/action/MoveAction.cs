using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

//Action for moving to tile. Has no check or logic.
namespace game.model.component.task.action {
// TODO visual bug: when giving order to move while moving, sprite positions and movement target not updated immediately
    public class MoveAction : Action {

        public MoveAction(Vector3Int targetPosition) : base(new PositionActionTarget(targetPosition, ActionTargetTypeEnum.EXACT)) {
            name = "Move to " + targetPosition;
            startCondition = () => ActionCheckingEnum.OK;
            finishCondition = () => true;
        }

        public string toString() {
            return "Move name";
        }
    }
}
