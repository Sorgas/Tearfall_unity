using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit;

namespace generation.unit {
    public class UnitEquipmentComponentGenerator {
        private readonly Random random;

        public UnitEquipmentComponentGenerator(Random random) {
            this.random = random;
        }

        public UnitEquipmentComponent generate(CreatureType type) {
            var component = new UnitEquipmentComponent();
            component.slots = type.slots
                .ToDictionary(pair => pair.Key, pair => new WearEquipmentSlot(pair.Key, pair.Value));
            component.grabSlots = type.grabSlots
                .ToDictionary(pair => pair.Key, pair => new GrabEquipmentSlot(pair.Key, pair.Value));
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