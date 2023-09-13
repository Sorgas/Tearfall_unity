using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action;
using game.model.component.task.action.equipment.use;
using game.model.component.unit;
using game.model.localmap;
using game.model.system.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.item;
using util.lang.extension;

namespace types.unit.need {
// need for wearing clothes. creates action for equipping wear items first for desired slots with high priority, and then to other slots with medium priority
public class WearNeed : Need {
    public override int getPriority(float value) {
        // TODO health needs if desired slots are empty, JOB for other slots
        return TaskPriorities.HEALTH_NEEDS;
    }

    public override Action tryCreateAction(LocalModel model, EcsEntity unit) {
        if (!unit.Has<UnitCalculatedWearNeedComponent>()) return null;
        UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
        ItemSelector selector = new WearWithSlotItemSelector(wear.desiredSlotsToFill);
        EcsEntity item = model.itemContainer.findingUtil.findNearestItemBySelector(selector, unit.pos()); // TODO select best item?
        if (item != EcsEntity.Null) return new EquipWearItemAction(item);
        return null;
    }

    public override UnitTaskAssignment createDescriptor(LocalModel model, EcsEntity unit) {
        if (!unit.Has<UnitCalculatedWearNeedComponent>()) return null;
        UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
        EcsEntity desiredItem = findItemForSlots(model, wear.desiredSlotsToFill, unit.pos());
        if (desiredItem != EcsEntity.Null)
            return new UnitTaskAssignment(desiredItem, model.itemContainer.getItemAccessPosition(desiredItem), "wear", unit, TaskPriorities.HEALTH_NEEDS);
        EcsEntity undesiredItem = findItemForSlots(model, wear.otherSlotsToFill, unit.pos());
        if (undesiredItem != EcsEntity.Null)
            return new UnitTaskAssignment(undesiredItem, model.itemContainer.getItemAccessPosition(undesiredItem), "wear", unit, TaskPriorities.JOB);
        return null;
    }

    public EcsEntity findItemForSlots(LocalModel model, List<string> slots, Vector3Int unitPosition) {
        ItemSelector selector = new WearWithSlotItemSelector(slots);
        return model.itemContainer.findingUtil.findNearestItemBySelector(selector, unitPosition); // TODO select best item?
    }
}
}