using enums.unit.need;
using game.model.component.unit;
using Leopotam.Ecs;

namespace game.model.system.unit
{
    // rolls needs counters in NeedsComponent (hunger, thirst, rest)
    // needs values define priorities for task to fullfill need
    public class UnitNeedSystem : EcsRunIntervalSystem {
        public const int interval = GameTime.minute * 5;
        private readonly float restTick;
        private EcsFilter<UnitNeedComponent> filter;
        private int counter;

        public UnitNeedSystem() : base(interval) { 
            restTick = 1f / Needs.rest.getHoursTo0() / GameTime.hour * interval;
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
            // component.hunger -= hungerTick;
            // component.thirst += 1;
            component.rest -= restTick;
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