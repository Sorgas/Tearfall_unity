using game.model.component.building;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using util.lang.extension;

namespace game.model.component.task.action.equipment.use {
    public class PutItemToContainerAction : PutItemToDestinationAction {

        public PutItemToContainerAction(EcsEntity containerEntity, EcsEntity item) : 
            base(new EntityActionTarget(containerEntity, ActionTargetTypeEnum.NEAR), item) {
            name = "put item to container";
            onFinish = () => {
                equipment().hauledItem = EcsEntity.Null;
                containerEntity.take<BuildingItemContainerComponent>().items.Add(item);
                model.itemContainer.transition.fromUnitToContainer(item, performer, containerEntity);
            };
        }
    }
}