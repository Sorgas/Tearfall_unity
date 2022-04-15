using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.unit {
    // checks slots required for unit to be filled, and updates need in UnitNeedComponent
    public class UnitWearNeedSystem : EcsRunIntervalSystem {
        private EcsFilter<UnitWearNeedComponent>.Exclude<UnitCalculatedWearNeedComponent> filter;

        public UnitWearNeedSystem() : base(100) { }

        public override void runLogic() {
            foreach (var i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                ref UnitWearNeedComponent wearNeed = ref filter.Get1(i);
                if (!wearNeed.valid) {
                    List<string> slots = findSlots(ref wearNeed, ref unit);
                    if (slots.Count > 0) {
                        unit.Replace(new UnitCalculatedWearNeedComponent { slotsToFill = slots });
                        Debug.Log("wear need added");
                    } else {
                        Debug.Log("all slots filled");
                    }
                    wearNeed.valid = true; // wear need added
                }
            }
        }

        // check slots in UnitEquipmentComponent
        private List<string> findSlots(ref UnitWearNeedComponent component, ref EcsEntity unit) {
            // todo add flag for updating(reset flag when equipment changes)
            if (unit.Has<UnitEquipmentComponent>()) {
                UnitEquipmentComponent equipment = unit.takeRef<UnitEquipmentComponent>();
                return component.desiredSlots
                    .Select(slot => equipment.slots[slot])
                    .Where(slot => slot.item == EcsEntity.Null)
                    .Select(slot => slot.name).ToList();
            }
            Debug.LogError("Unit with wear need, but without EquipmentComponent");
            return new List<string>();
        }

    }
}