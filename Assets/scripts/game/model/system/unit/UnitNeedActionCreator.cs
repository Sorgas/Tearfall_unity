using game.model.component.task.action;
using game.model.component.task.action.equipment.use;
using game.model.component.unit;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using types.unit.need;
using util.item;
using util.lang.extension;
using static types.action.TaskPriorities;

namespace game.model.system.unit {
    public class UnitNeedActionCreator {
        private static TaskPriorities[] priorities = {SAFETY, HEALTH_NEEDS, COMFORT};

        //TODO add other needs
        public EcsEntity selectAndCreateAction(LocalModel model, EcsEntity unit) {
            foreach(TaskPriorities priority in priorities) {
                Action action = createActionForPriority(model, unit, priority);
                if(action != null) return model.taskContainer.generator.createTask(action, priority, model.createEntity(), model);
            }
            return EcsEntity.Null;
        }

        private Action createActionForPriority(LocalModel model, EcsEntity unit, TaskPriorities priority) {
            if (priority == TaskPriorities.HEALTH_NEEDS && unit.Has<UnitCalculatedWearNeedComponent>()) {
                UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
                ItemSelector selector = new WearWithSlotItemSelector(wear.slotsToFill);
                EcsEntity item = model.itemContainer.util.findFreeReachableItemBySelector(selector, unit.pos()); // TODO select best item?
                if (!item.IsNull()) {
                    return new EquipWearItemAction(item);
                }
            }
            if (unit.Has<UnitNeedComponent>()) {
                UnitNeedComponent needs = unit.take<UnitNeedComponent>();
                Action action = null;
                if(needs.hungerPriority == priority) {
                    action = Needs.hunger.tryCreateAction(model, unit);
                }
                if (needs.restPriority == priority && action == null) {
                    action = Needs.rest.tryCreateAction(model, unit);
                }
                if(action != null) return action;
            }
            return null;
        }
    }
}