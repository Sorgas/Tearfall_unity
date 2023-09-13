using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action.equipment.use;
using game.model.component.unit;
using game.model.localmap;
using Leopotam.Ecs;
using MoreLinq;
using types.action;
using types.unit.need;
using util.item;
using util.lang.extension;
using static types.action.TaskPriorities;
using Action = game.model.component.task.action.Action;

namespace game.model.system.unit {
// checks all needs of unit in priority order. Returns performable action for of most prioritized need
public class UnitNeedActionCreator {

    //TODO add other needs
    public Action selectAndCreateAction(LocalModel model, EcsEntity unit) {
        for (int priority = range.max; priority >= range.min; priority--) {
            Action action = createActionForPriority(model, unit, priority);
            if (action != null) return action;
            // model.taskContainer.generator.createTask(action, Jobs.NONE, priority, model.createEntity(), model);
        }
        return null;
    }

    private Action createActionForPriority(LocalModel model, EcsEntity unit, int priority) {
        if (priority == HEALTH_NEEDS && unit.Has<UnitCalculatedWearNeedComponent>()) {
            UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
            ItemSelector selector = new WearWithSlotItemSelector(wear.desiredSlotsToFill);
            EcsEntity item = model.itemContainer.findingUtil.findNearestItemBySelector(selector, unit.pos()); // TODO select best item?
            if (item != EcsEntity.Null) {
                return new EquipWearItemAction(item);
            }
        }
        if (unit.Has<UnitNeedComponent>()) {
            UnitNeedComponent needs = unit.take<UnitNeedComponent>();
            Action action = null;
            if (needs.hungerPriority == priority) {
                action = Needs.hunger.tryCreateAction(model, unit);
            }
            if (needs.restPriority == priority && action == null) {
                action = Needs.rest.tryCreateAction(model, unit);
            }
            // TODO add thirst
            if (action != null) return action;
        }
        return null;
    }

    public UnitTaskAssignment getMaxPriorityPerformableNeedAction(LocalModel model, EcsEntity unit) {
        List<UnitTaskAssignment> list = new();
        list.Add(Needs.wear.createDescriptor(model, unit));
        list.Add(Needs.rest.createDescriptor(model, unit));
        list.Add(Needs.hunger.createDescriptor(model, unit));
        return list.MaxBy(descriptor => descriptor?.performer.priority ?? NONE);
    }
}
}