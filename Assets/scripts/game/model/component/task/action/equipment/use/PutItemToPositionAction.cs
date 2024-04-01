using game.model.component.task.action.target;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;

namespace game.model.component.task.action.equipment.use {
    // puts item from unit's hands to ground tile. check is made in superclass
    public class PutItemToPositionAction : PutItemToDestinationAction {
        public PutItemToPositionAction(EcsEntity item, Vector3Int targetPosition) : base(new PositionActionTarget(targetPosition, ANY), item) {
            name = "put item " + item.name() + " to position " + targetPosition;
            
            onFinish = () => {
                equipment.hauledItem = EcsEntity.Null;
                model.itemContainer.transition.fromUnitToGround(item, performer, targetPosition);
                unlockEntity(item);
            };
        }
    }
}