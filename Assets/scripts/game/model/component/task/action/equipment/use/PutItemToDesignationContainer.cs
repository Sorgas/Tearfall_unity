using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using util.lang.extension;

namespace game.model.component.task.action.equipment.use {
    public class PutItemToDesignationContainer: PutItemToDestinationAction {

        public PutItemToDesignationContainer(EcsEntity containerEntity, EcsEntity item) : 
            base(new EntityActionTarget(containerEntity, ActionTargetTypeEnum.NEAR), item) {
            onFinish = () => {
                equipment().hauledItem = EcsEntity.Null;
                containerEntity.take<DesignationItemContainerComponent>().items.Add(item);
            };
        }
    }
}