using enums.action;
using game.model.component.task.action.target;
using Leopotam.Ecs;

namespace game.model.component.task.action.equipment.use {
    public class PutItemToContainerAction : PutItemToDestinationAction {

        public PutItemToContainerAction(EcsEntity containerEntity, EcsEntity item) : 
            base(new EntityActionTarget(containerEntity, ActionTargetTypeEnum.NEAR), item) {
            onFinish = () => {
                equipment().hauledItem = EcsEntity.Null;
                GameModel.get().itemContainer.transition.fromUnitToContainer(item, performer, containerEntity);
            };
        }
    }
}