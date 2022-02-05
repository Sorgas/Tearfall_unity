using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using Leopotam.Ecs;

namespace game.model.component.unit {
    public struct UnitEquipmentComponent {
        public Dictionary<string, EquipmentSlot> slots; // all slots of a creature (for wear)
        public Dictionary<string, GrabEquipmentSlot> grabSlots; // slots for tools (subset of all slots)
        public HashSet<EcsEntity> items; // items in worn containers and in hands
        public List<EquipmentSlot> desiredSlots; // uncovered limbs give comfort penalty
        public EcsEntity? hauledItem;

        //    /**
        // * Filters and returns slots needed to be filled to avoid nudity penalty.
        // */
        //    public Stream<EquipmentSlot> getEmptyDesiredSlots() {
        //        return desiredSlots.stream().filter(equipmentSlot->equipmentSlot.item == null);
        //    }

        public bool toolWithActionEquipped(String action) {
            return grabSlots.Values
                .Where(slot => slot.isToolGrabbed())
                .Select(slot => slot.grabbedItem.Value.Get<ItemToolComponent>())
                .Count(tool => tool.operation == action) > 0;
        }

        // public Optional<EquipmentSlot> slotWithItem(Progress.Item item) {
        //     return slotStream().filter(slot->slot.item == item).findFirst();
        // }
        //
        // public Optional<GrabEquipmentSlot> grabSlotWithItem(Progress.Item item) {
        //     return grabSlotStream().filter(slot->slot.grabbedItem == item).findFirst();
        // }
        //
        // public Stream<EquipmentSlot> slotStream() {
        //     return slots.values().stream();
        // }
        //
        // public Stream<GrabEquipmentSlot> grabSlotStream() {
        //     return grabSlots.values().stream();
        // }
    }

    public class EquipmentSlot {
        public string name;
        public EcsEntity item; //TODO mvp single item, add layers
        public List<String> limbs; // limbs covered by items in this slot. items can cover additional limbs

        public EquipmentSlot(String name, List<string> limbs) {
            this.name = name;
            this.limbs = new List<string>(limbs);
        }
    }

    public class GrabEquipmentSlot : EquipmentSlot {
        public EcsEntity? grabbedItem; // null, if free

        public GrabEquipmentSlot(String name, List<string> limbs) : base(name, limbs) { }

        public bool isGrabFree() {
            return !grabbedItem.HasValue;
        }

        public bool isToolGrabbed() {
            return grabbedItem.HasValue && grabbedItem.Value.Has<ItemToolComponent>();
        }
    }
}