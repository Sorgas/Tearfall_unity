using game.model.component.unit;
using Leopotam.Ecs;
using types.unit.need;

namespace game.model.system.unit {
    // rolls needs counters in NeedsComponent (hunger, thirst, rest)
    // updates needs priorities
    // if priority changes, task delay is reset
    public class UnitNeedSystem : EcsRunIntervalSystem {
        public const int interval = GameTime.ticksPerMinute * 5;
        private readonly float restTick;
        private readonly float hungerTick;
        private EcsFilter<UnitNeedComponent> filter;
        private int counter;

        public UnitNeedSystem() : base(interval) {
            restTick = 1f / RestNeed.hoursToSafety / GameTime.ticksPerHour * interval;
            hungerTick = 1f / HungerNeed.hoursToSafety / GameTime.ticksPerHour * interval;
        }

        public override void runLogic() {
            foreach (var i in filter) {
                ref UnitNeedComponent component = ref filter.Get1(i);
                ref EcsEntity unit = ref filter.GetEntity(i);
                rollNeeds(ref component);
                // updateSlots(ref component, ref unit);
            }
        }

        private void rollNeeds(ref UnitNeedComponent component) {
            component.hunger -= hungerTick;
            if(component.hunger < 0) component.hunger = 0;
            component.hungerPriority = Needs.hunger.getPriority(component.hunger);
            // component.thirst += 1;
            component.rest -= restTick;
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