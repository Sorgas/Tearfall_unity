using game.model.component.item;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using util.lang.extension;

// Action for taking item from container. Locks item. Item should not be locked to another task
namespace game.model.component.task.action.equipment.obtain {
public class GetItemFromContainerAction : ItemAction {
    public GetItemFromContainerAction(EcsEntity item) : base(null, item) {
        name = "get " + item.name() + " from container";
        EcsEntity containerEntity = item.take<ItemContainedComponent>().container;
        target = new EntityActionTarget(containerEntity, ActionTargetTypeEnum.NEAR);
        maxProgress = 50;

        startCondition = () => {
            if (!validate()) return ActionCheckingEnum.FAIL;
            // setItemLocked(item, true);
            // drop current item
            if (equipment.hauledItem != EcsEntity.Null)
                return addPreAction(new PutItemToPositionAction(equipment.hauledItem, performer.pos()));
            return ActionCheckingEnum.OK;
        };

        onFinish = () => {
            container.transition.fromContainerToUnit(item, containerEntity, performer);
            equipment.hauledItem = item;
            log(item + " got from container");
        };
    }

    // Adds validation of container existence, content and reachability.
    private new bool validate() {
        EcsEntity containerEntity = item.take<ItemContainedComponent>().container;
        ItemContainerComponent containerComponent = containerEntity.take<ItemContainerComponent>(); ;
        return base.validate()
               && containerComponent.items.Contains(item)
               && model.localMap.passageMap.inSameArea(model.itemContainer.getItemAccessPosition(item), performer.pos());
    }
}
}