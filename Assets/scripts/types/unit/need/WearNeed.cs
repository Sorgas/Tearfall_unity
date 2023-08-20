using game.model.component.task.action;
using game.model.component.task.action.equipment.use;
using game.model.component.unit;
using game.model.localmap;
using game.model.system.unit;
using Leopotam.Ecs;
using types.action;
using util.item;
using util.lang.extension;

namespace types.unit.need {
public class WearNeed : Need {

    public WearNeed() : base() { }

    public override int getPriority(float value) {
        return TaskPriorities.HEALTH_NEEDS;
    }
    public override Action tryCreateAction(LocalModel model, EcsEntity unit) {
        if (!unit.Has<UnitCalculatedWearNeedComponent>()) return null;
        UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
        ItemSelector selector = new WearWithSlotItemSelector(wear.slotsToFill);
        EcsEntity item = model.itemContainer.findingUtil.findNearestItemBySelector(selector, unit.pos()); // TODO select best item?
        if (item != EcsEntity.Null) return new EquipWearItemAction(item);
        return null;
    }
    public override TaskAssignmentDescriptor createDescriptor(LocalModel model, EcsEntity unit) {
        if (!unit.Has<UnitCalculatedWearNeedComponent>()) return null;
        UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
        ItemSelector selector = new WearWithSlotItemSelector(wear.slotsToFill);
        EcsEntity item = model.itemContainer.findingUtil.findNearestItemBySelector(selector, unit.pos()); // TODO select best item?
        if (item != EcsEntity.Null) 
            return new TaskAssignmentDescriptor(item, model.itemContainer.getItemAccessPosition(item), "wear", unit, TaskPriorities.HEALTH_NEEDS);
        return null;
    }


    //
    // public override boolean isSatisfied(CanvasScaler.Unit unit) {
    //     throw new System.NotImplementedException();
    // }
    //
    // public override Task tryCreateTask(CanvasScaler.Unit unit) {
    //     throw new System.NotImplementedException();
    // }
    //
    // public override MoodEffect getMoodPenalty(CanvasScaler.Unit unit, NeedState state) {
    //     throw new System.NotImplementedException();
    // }
}
}