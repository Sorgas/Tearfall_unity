using Leopotam.Ecs;

namespace game.model.component.task.action.equipment.use {
    // puts item into item container e.g. chest
    public class PutItemToContainerAction : PutItemToDestinationAction {

        public PutItemToContainerAction(EcsEntity containerEntity, EcsEntity item) : base(resolveItemContainerTarget(containerEntity), item) {
            name = "put item to container";
            onFinish = () => {
                equipment.hauledItem = EcsEntity.Null;
                model.itemContainer.transition.fromUnitToContainer(item, performer, containerEntity);
            };
        }
    }
}