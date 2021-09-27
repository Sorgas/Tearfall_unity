using Assets.scripts.enums.action;
using Assets.scripts.game.model;
using Leopotam.Ecs;
using UnityEngine;

//Action for moving to tile. Has no check or logic.
public class MoveAction : _Action {

    public MoveAction(Vector3Int targetPosition) : base(new PositionActionTarget(targetPosition, ActionTargetTypeEnum.EXACT)) {
        
        startCondition = (unit) => {
            if (unit.Has<MovementComponent>()) {
                Vector3Int currentPosition = unit.Get<MovementComponent>().position;
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
