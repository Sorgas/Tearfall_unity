using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;

namespace game.model.component.task.action.equipment.use {
    // puts item into item container e.g. chest
    public class PutItemToContainerAction : PutItemToDestinationAction {

        public PutItemToContainerAction(EcsEntity containerEntity, EcsEntity item) : 
            base(new EntityActionTarget(containerEntity, ActionTargetTypeEnum.NEAR), item) {
            name = "put item to container";
            onFinish = () => {
                equipment.hauledItem = EcsEntity.Null;
                model.itemContainer.transition.fromUnitToContainer(item, performer, containerEntity);
            };
        }
    }
}