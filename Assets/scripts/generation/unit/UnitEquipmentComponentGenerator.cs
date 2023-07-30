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
            component.grabSlots = new Dictionary<string, GrabEquipmentSlot>();
            foreach (var pair in type.slots) {
                EquipmentSlot slot;
                if (isGrabSlot(pair.Key, type)) {
                    slot = new GrabEquipmentSlot(pair.Key, pair.Value);
                    component.grabSlots.Add(pair.Key, (GrabEquipmentSlot)slot);
                } else {
                    slot = new EquipmentSlot(pair.Key, pair.Value);
                }
                component.slots.Add(pair.Key, slot);
            }
            component.items = new HashSet<EcsEntity>();
            return component;
        }

        // slot can grab items if any limb of it has "grab" tag
        private bool isGrabSlot(string slot, CreatureType type) {
            return type.slots[slot]
                .Select(slotLimb => type.bodyParts[slotLimb].tags)
                .Any(tags => tags.Contains("grab"));
        }
    }
}