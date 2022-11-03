using enums.action;
using game.model.component.building;
using game.model.component.item;
using game.model.component.task.action.equipment;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using util.lang.extension;

// Action for taking item from container. Locks item. Item should not be locked or be locked to same task
public class GetItemFromContainerAction : EquipmentAction {

    public GetItemFromContainerAction(EcsEntity item) : base(null, item) {
        EcsEntity containerEntity = item.take<ItemContainedComponent>().container;
        target = new EntityActionTarget(containerEntity, ActionTargetTypeEnum.NEAR);

        startCondition = () => {
            if (!validate()) return ActionConditionStatusEnum.FAIL;
            // setItemLocked(item, true);
            // drop current item
            if (equipment().hauledItem != null)
                return addPreAction(new PutItemToPositionAction(equipment().hauledItem, performer.pos()));
            return ActionConditionStatusEnum.OK;
        };

        onStart = () => maxProgress = 50;

        onFinish = () => {
            container.transition.fromContainerToUnit(item, containerEntity, performer);
            equipment().hauledItem = item;
            log(item + " got from container");
        };
    }

    // TODO 
    // Adds validation of container existence, content and reachability.
    protected bool validate() {
        EcsEntity containerEntity = item.take<ItemContainedComponent>().container;
        BuildingItemContainerComponent containerComponent = containerEntity.take<BuildingItemContainerComponent>();
        if (item.Has<ItemLockedComponent>()) return false;
        return base.validate()
                && containerComponent.items.Contains(item)
                // && model.localMap.passageMap.inSameArea(this.containerEntity.position, item.position);
                ;
    }
}