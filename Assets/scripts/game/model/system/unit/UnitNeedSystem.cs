using game.model.component.unit;
using Leopotam.Ecs;
using types.unit.need;
using UnityEngine;

namespace game.model.system.unit {
    // rolls needs counters in NeedsComponent (hunger, thirst, rest)
    // updates needs priorities
    public class UnitNeedSystem : LocalModelIntervalEcsSystem {
        private const int interval = GameTime.ticksPerMinute * 5; // every 5 in-game minutes
        private readonly float restTick;
        private readonly float hungerTick;
        private EcsFilter<UnitNeedComponent> filter;

        public UnitNeedSystem() : base(interval) {
            restTick = 1f / RestNeed.hoursToSafety / GameTime.ticksPerHour * interval;
            hungerTick = HungerNeed.perTickChange * interval;
        }
        
        protected override void runIntervalLogic(int updates) {
            foreach (var i in filter) {
                ref UnitNeedComponent component = ref filter.Get1(i);
                ref EcsEntity unit = ref filter.GetEntity(i);
                rollNeeds(ref component, updates);
                // updateSlots(ref component, ref unit);
            }
        }

        // rolls needs values from 1 to 0
        private void rollNeeds(ref UnitNeedComponent component, int updates) {
            component.hunger -= hungerTick * updates;
            if(component.hunger < 0) component.hunger = 0;
            component.hungerPriority = Needs.hunger.getPriority(component.hunger);
            // component.thirst += 1 * updates;
            component.rest -= restTick * updates;
            if(component.rest < 0) component.rest = 0;
            component.restPriority = Needs.rest.getPriority(component.rest);
        }

        // private void updateSlots(ref UnitNeedComponent component, ref EcsEntity unit) {
        //     // todo add flag for updating
        //     if (unit.Has<UnitEquipmentComponent>()) {
        //         UnitEquipmentComponent equipment = unit.take<UnitEquipmentComponent>();
        //         component.slotsToFill = component.desiredSlots
        //             .Select(slot => equipment.slots[slot])
        //             .Where(slot => slot.item == EcsEntity.Null)
        //             .Select(slot => slot.name).ToList();
        //     }
        // }
    }
}