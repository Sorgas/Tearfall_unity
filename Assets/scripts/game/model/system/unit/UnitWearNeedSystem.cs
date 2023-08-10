using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.unit {
    // checks slots required for unit to be filled, and creates UnitCalculatedWearNeedComponent
    public class UnitWearNeedSystem : LocalModelIntervalEcsSystem {
        private EcsFilter<UnitWearNeedComponent>.Exclude<UnitCalculatedWearNeedComponent> filter;

        public UnitWearNeedSystem() : base(100) {
            name = "UnitWearNeedSystem";
        }

        protected override void runIntervalLogic(int updates) {
            foreach (var i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                ref UnitWearNeedComponent wearNeed = ref filter.Get1(i);
                if (!wearNeed.valid) {
                    List<string> slots = findSlots(ref wearNeed, ref unit);
                    if (slots.Count > 0) {
                        unit.Replace(new UnitCalculatedWearNeedComponent { slotsToFill = slots });
                        log($"wear need added for {unit.name()}");
                    } else {
                        log("all slots filled");
                    }
                    wearNeed.valid = true; // wear need added
                }
            }
        }

        // check slots in UnitEquipmentComponent
        private List<string> findSlots(ref UnitWearNeedComponent component, ref EcsEntity unit) {
            // TODO add flag for updating(reset flag when equipment changes)
            // TODO make delay when equipment not available
            if (unit.Has<UnitEquipmentComponent>()) {
                UnitEquipmentComponent equipment = unit.takeRef<UnitEquipmentComponent>();
                return component.desiredSlots
                    .Select(slot => equipment.slots[slot])
                    .Where(slot => slot.item == EcsEntity.Null)
                    .Select(slot => slot.name).ToList();
            }
            Debug.LogError("Unit has UnitWearNeedComponent, but no EquipmentComponent");
            return new List<string>();
        }

    }
}