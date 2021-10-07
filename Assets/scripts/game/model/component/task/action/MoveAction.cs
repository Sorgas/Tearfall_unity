using enums.action;
using game.model.component.task.action.target;
using game.model.component.unit.components;
using Leopotam.Ecs;
using UnityEngine;

//Action for moving to tile. Has no check or logic.
namespace game.model.component.task.action {
    public class MoveAction : _Action {

        public MoveAction(Vector3Int targetPosition) : base(new PositionActionTarget(targetPosition, ActionTargetTypeEnum.EXACT)) {
        
            startCondition = (unit) => {
                if (unit.Has<MovementComponent>()) {
                    Vector3Int currentPosition = unit.Get<MovementComponent>().position;
                    Debug.Log("checking in same area");
                    if (GameModel.get().localMap.passageMap.inSameArea(currentPosition, targetPosition)) {
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
