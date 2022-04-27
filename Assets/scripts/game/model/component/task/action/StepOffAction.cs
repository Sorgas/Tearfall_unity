using System.Collections.Generic;
using System.Linq;
using enums.action;
using game.model.component.task.action.target;
using game.model.localmap.passage;
using UnityEngine;

namespace game.model.component.task.action {
    public class StepOffAction : Action {

        public StepOffAction(Vector3Int from) : base(new PositionActionTarget(from, ActionTargetTypeEnum.EXACT)) {
            Vector3Int targetPos = findPositionToStepInto(from);
            if (targetPos.z != -1) {
                startCondition = () => ActionConditionStatusEnum.OK;
                target = new PositionActionTarget(targetPos, ActionTargetTypeEnum.EXACT);
            }
        }

        private Vector3Int findPositionToStepInto(Vector3Int from) {
            List<Vector3Int> positions = new NeighbourPositionStream(from).filterConnectedToCenter().stream.ToList();
            if (positions.Count > 0) {
                return positions[0];
            }
            return new Vector3Int(0, 0, -1);
        }
    }
}