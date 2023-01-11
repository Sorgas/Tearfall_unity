using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit;

namespace generation.unit {
    public class UnitEquipmentComponentGenerator {

        public UnitEquipmentComponent generate(CreatureType type) {
            var component = new UnitEquipmentComponent();
            component.slots = new Dictionary<string, EquipmentSlot>();
            foreach (var pair in type.slots) {
                EquipmentSlot slot = isGrabSlot(pair.Key, type)
                    ? new GrabEquipmentSlot(pair.Key, pair.Value)
                    : new EquipmentSlot(pair.Key, pair.Value);
                component.slots.Add(pair.Key, slot);
            }
            component.grabSlots = new Dictionary<string, GrabEquipmentSlot>();
            component.items = new HashSet<EcsEntity>();
            fillSlots(ref component);
            return component;
        }

        private void fillSlots(ref UnitEquipmentComponent component) {
            component.grabSlots.Add("left", new GrabEquipmentSlot("left", new List<string>()));
        }

        private bool isGrabSlot(string slot, CreatureType type) {
            return type.slots[slot]
                .Select(slotLimb => type.bodyParts[slotLimb].tags)
                .Any(tags => tags.Contains("grab"));
        }
    }
}