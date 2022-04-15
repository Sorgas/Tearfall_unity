using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.system.unit {
    // rolls needs counters in NeedsComponent (hunger, thirst, rest)
    // checks unit's equipment, health condition, etc and fills NeedsComponent
    public class UnitNeedSystem : IEcsRunSystem {
        // private EcsFilter<UnitNeedComponent> filter;
        // private int counter;
        
        public void Run() {
            // if (counter++ >= 100) {
            //     counter = 0;
            //     foreach (var i in filter) {
            //         ref UnitNeedComponent component = ref filter.Get1(i);
            //         ref EcsEntity unit = ref filter.GetEntity(i);
            //         rollNeeds(ref component);
            //         updateSlots(ref component, ref unit);
            //         calculateNeeds(ref component);
            //     }
            // }
        }
        
        // private void rollNeeds(ref UnitNeedComponent component) {
        //     component.hunger += 1;
        //     component.thirst += 1;
        //     component.sleep += 1;
        // }
        //
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
        //
        // private void calculateNeeds(ref UnitNeedComponent component) {
        //     
        // }
    }
}