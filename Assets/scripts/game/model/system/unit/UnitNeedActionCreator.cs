using game.model.component.task.action;
using game.model.component.task.action.equipment.use;
using game.model.component.unit;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using types.unit;
using types.unit.need;
using util.item;
using util.lang.extension;
using static types.action.TaskPriorities;

namespace game.model.system.unit {
    // checks all needs of unit in priority order. Returns performable action for of most prioritized need
    public class UnitNeedActionCreator {
        private static int[] priorities = {SAFETY, HEALTH_NEEDS, COMFORT};

        //TODO add other needs
        public EcsEntity selectAndCreateAction(LocalModel model, EcsEntity unit) {
            for (int priority = range.max; priority >= range.min; priority--) {
                
            }
            foreach(int priority in priorities) {
                Action action = createActionForPriority(model, unit, priority);
                if(action != null) return model.taskContainer.generator.createTask(action, Jobs.NONE, priority, model.createEntity(), model);
            }
            return EcsEntity.Null;
        }

        private Action createActionForPriority(LocalModel model, EcsEntity unit, int priority) {
            if (priority == HEALTH_NEEDS && unit.Has<UnitCalculatedWearNeedComponent>()) {
                UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
                ItemSelector selector = new WearWithSlotItemSelector(wear.slotsToFill);
                EcsEntity item = model.itemContainer.findingUtil.findNearestItemBySelector(selector, unit.pos()); // TODO select best item?
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