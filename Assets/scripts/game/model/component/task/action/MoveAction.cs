using enums.action;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

//Action for moving to tile. Has no check or logic.
namespace game.model.component.task.action {
    public class MoveAction : Action {

        public MoveAction(Vector3Int targetPosition) : base(new PositionActionTarget(targetPosition, ActionTargetTypeEnum.EXACT)) {
        
            startCondition = () => {
                if (performer.Has<UnitMovementComponent>()) {
                    Vector3Int currentPosition = performer.pos();
                    Debug.Log("checking in same area");
                    if (GameModel.localMap.passageMap.inSameArea(currentPosition, targetPosition)) {
                        return ActionConditionStatusEnum.OK;
                    }
                }
                Debug.LogWarning("Creature cannot move to " + targetPosition);
                return ActionConditionStatusEnum.FAIL;
            };
            finishCondition = () => true;
        }

        public string toString() {
            return "Move name";
        }
    }
}
