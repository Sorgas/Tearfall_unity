using enums.action;
using enums.unit.need;
using game.model.component.task.action;
using game.model.component.task.action.equipment.use;
using game.model.component.unit;
using Leopotam.Ecs;
using util.item;
using util.lang.extension;
using static enums.action.TaskPriorityEnum;

public class UnitNeedActionCreator {
    private static TaskPriorityEnum[] priorities = {SAFETY, HEALTH_NEEDS, COMFORT};

    //TODO add other needs
    public EcsEntity selectAndCreateAction(LocalModel model, EcsEntity unit) {
        foreach(TaskPriorityEnum priority in priorities) {
            Action action = createActionForPriority(model, unit, priority);
            if(action != null) return model.taskContainer.generator.createTask(action, priority, model.createEntity(), model);
        }
        return EcsEntity.Null;
    }

    private Action createActionForPriority(LocalModel model, EcsEntity unit, TaskPriorityEnum priority) {
        if (priority == TaskPriorityEnum.HEALTH_NEEDS && unit.Has<UnitCalculatedWearNeedComponent>()) {
            UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
            ItemSelector selector = new WearWithSlotItemSelector(wear.slotsToFill);
            EcsEntity item = model.itemContainer.util.findFreeReachableItemBySelector(selector, unit.pos()); // TODO select best item?
            if (!item.IsNull()) {
                return new EquipWearItemAction(item);
            }
        }
        if (unit.Has<UnitNeedComponent>()) {
            UnitNeedComponent needs = unit.take<UnitNeedComponent>();
            if (needs.restPriority == priority) {
                return Needs.rest.tryCreateAction(model, unit);
            }
        }
        return null;
    }
}