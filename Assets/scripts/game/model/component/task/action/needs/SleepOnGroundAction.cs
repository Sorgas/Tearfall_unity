using enums.action;
using game.model.component.task.action;
using game.model.component.task.action.target;
using UnityEngine;

public class SleepOnGroundAction : Action {
    public SleepOnGroundAction(Vector3Int position) : base(new PositionActionTarget(position, ActionTargetTypeEnum.EXACT)) {
    
    }
}