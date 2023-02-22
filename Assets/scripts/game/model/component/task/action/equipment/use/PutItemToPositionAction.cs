using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using UnityEngine;

namespace game.model.component.task.action.equipment.use {
    public class PutItemToPositionAction : PutItemToDestinationAction {
        public PutItemToPositionAction(EcsEntity item, Vector3Int targetPosition) : base(new PositionActionTarget(targetPosition, ActionTargetTypeEnum.ANY), item) {
            name = "put item to position";
            onFinish = () => {
                equipment().hauledItem = EcsEntity.Null;
                // TODO put item to map
                // TODO add component ItemPutToGroundComponent
                // GameModel.get().itemContainer.
            };
        }
    }
}