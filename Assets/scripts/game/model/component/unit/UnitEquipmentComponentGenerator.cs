using System.Collections.Generic;
using Leopotam.Ecs;

namespace game.model.component.unit {
    public class UnitEquipmentComponentGenerator {

        public UnitEquipmentComponent generate() {
            var component = new UnitEquipmentComponent();
            component.slots = new Dictionary<string, EquipmentSlot>();
            component.grabSlots = new Dictionary<string, GrabEquipmentSlot>();
            component.items = new HashSet<EcsEntity>();
            component.desiredSlots = new List<EquipmentSlot>();
            fillSlots(ref component);
            return component;
        }

        private void fillSlots(ref UnitEquipmentComponent component) {
            component.grabSlots.Add("left", new GrabEquipmentSlot("left", new ));
        }
    }
}  