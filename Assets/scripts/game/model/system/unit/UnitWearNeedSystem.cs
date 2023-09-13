using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.system.unit {
// if unit has no UnitCalculatedWearNeedComponent, this system observes unit's equipment and creates this component.
// This component is then removed when any update of equipment is made
public class UnitWearNeedSystem : LocalModelIntervalEcsSystem {
    private EcsFilter<UnitWearNeedComponent>.Exclude<UnitCalculatedWearNeedComponent> filter;

    public UnitWearNeedSystem() : base(100) {
        name = "UnitWearNeedSystem";
        debug = true;
    }

    protected override void runIntervalLogic(int updates) {
        foreach (var i in filter) {
            EcsEntity unit = filter.GetEntity(i);
            UnitWearNeedComponent wearNeed = filter.Get1(i);
            unit.Replace(createWearNeedComponent(unit, wearNeed.desiredSlots));
            log($"wear need calculated for {unit.name()}");
        }
    }

    private UnitCalculatedWearNeedComponent createWearNeedComponent(EcsEntity unit, List<string> desiredSlots) {
        UnitEquipmentComponent equipment = unit.take<UnitEquipmentComponent>();
        List<string> desired = desiredSlots
            .Where(slot => equipment.slots[slot].item == EcsEntity.Null)
            .ToList();
        List<string> other = equipment.slots.Keys
            .Where(slot => !desiredSlots.Contains(slot))
            .Where(slot => equipment.slots[slot].item == EcsEntity.Null)
            .ToList();
        return new UnitCalculatedWearNeedComponent {
            desiredSlotsToFill = desired,
            otherSlotsToFill = other
        };
    }
}
}