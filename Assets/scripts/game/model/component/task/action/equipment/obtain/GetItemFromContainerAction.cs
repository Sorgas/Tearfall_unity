using game.model.component.item;
using game.model.component.task.action.equipment.use;
using Leopotam.Ecs;
using types.action;
using util.lang.extension;

// Action for taking item from container. Locks item. Item should not be locked to another task
namespace game.model.component.task.action.equipment.obtain {
public class GetItemFromContainerAction : ItemAction {
    private EcsEntity containerEntity;
    
    public GetItemFromContainerAction(EcsEntity item) : base(resolveItemContainerTarget(item.take<ItemContainedComponent>().container), item) {
        name = $"get {item.name()} from container";
        containerEntity = item.take<ItemContainedComponent>().container;
        maxProgress = 50;

        startCondition = () => {
            if (!validate()) return ActionCheckingEnum.FAIL;
            lockEntity(item);
            if (equipment.hauledItem != EcsEntity.Null)
                return addPreAction(new PutItemToPositionAction(equipment.hauledItem, performer.pos()));
            return ActionCheckingEnum.OK;
        };

        onFinish = () => {
            container.transition.fromContainerToUnit(item, containerEntity, performer);
            equipment.hauledItem = item;
            log($"{item} got from container");
        };
    }

    // Adds validation of container existence, cont ent and reachability.
    private new bool validate() {
        return base.validate()
               && containerEntity.take<ItemContainerComponent>().items.Contains(item)
               && model.localMap.passageMap.inSameArea(model.itemContainer.getItemAccessPosition(item), performer.pos());
    }
}
}