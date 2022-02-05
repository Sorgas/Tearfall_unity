using System.Collections.Generic;
using Leopotam.Ecs;

namespace game.model.component.unit {
    public class UnitEquipmentComponentGenerator {

        public UnitEquipmentComponent generate() {
            return new UnitEquipmentComponent {
                slots = new Dictionary<string, EquipmentSlot>(),
                grabSlots = new Dictionary<string, GrabEquipmentSlot>(),
                items = new HashSet<EcsEntity>(), 
                desiredSlots = new List<EquipmentSlot>()
            };
        }
    }
}