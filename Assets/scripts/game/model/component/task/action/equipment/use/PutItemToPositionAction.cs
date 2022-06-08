using enums.action;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.component.task.action.equipment.use {
    public class PutItemToPositionAction : PutItemToDestinationAction {
        public PutItemToPositionAction(EcsEntity item, Vector3Int targetPosition) : base(new PositionActionTarget(targetPosition, ActionTargetTypeEnum.ANY), item) {

            onFinish = () => {
                equipment().hauledItem = EcsEntity.Null;
                // TODO put item to map
                // GameModel.get().itemContainer.
            };
        }
    }
}