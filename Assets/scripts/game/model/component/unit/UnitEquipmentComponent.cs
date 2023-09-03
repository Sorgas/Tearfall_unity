using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using Leopotam.Ecs;

namespace game.model.component.unit {
// contains slots for wearable and holdable items (clothes and tools)
public struct UnitEquipmentComponent {
    public Dictionary<string, WearEquipmentSlot> slots; // all slots of a creature (for wear)
    public Dictionary<string, GrabEquipmentSlot> grabSlots; // slots for tools (subset of all slots)
    public HashSet<EcsEntity> items; // items in worn containers and in hands
    public bool desiredSlotsFilled;
    public EcsEntity hauledItem;

    //    /**
    // * Filters and returns slots needed to be filled to avoid nudity penalty.
    // */
    //    public Stream<EquipmentSlot> getEmptyDesiredSlots() {
    //        return desiredSlots.stream().filter(equipmentSlot->equipmentSlot.item == null);
    //    }

    public bool toolWithActionEquipped(String action) {
        return grabSlots.Values
            .Where(slot => slot.isToolGrabbed())
            .Select(slot => slot.item.Get<ItemToolComponent>())
            .Count(tool => tool.action == action) > 0;
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

// abstract slot depends on some limbs of unit's body. 
public abstract class AbstractEquipmentSlot {
    public readonly string name;
    public readonly List<string> limbs = new();

    protected AbstractEquipmentSlot(string name, List<string> limbs) {
        this.name = name;
        this.limbs.AddRange(limbs);
    }
}

// for wearing items like clothes and armor
public class WearEquipmentSlot : AbstractEquipmentSlot {
    public EcsEntity item = EcsEntity.Null; //TODO mvp single item, add layers
    
    public WearEquipmentSlot(string name, List<string> limbs) : base(name, limbs) { }
}

// for holding items like tools and weapons
public class GrabEquipmentSlot : AbstractEquipmentSlot {
    public EcsEntity item = EcsEntity.Null;

    public GrabEquipmentSlot(string name, List<string> limbs) : base(name, limbs) { }

    public bool isGrabFree() {
        return item == EcsEntity.Null;
    }

    public bool isToolGrabbed() {
        return item != EcsEntity.Null && item.Has<ItemToolComponent>();
    }
}
}