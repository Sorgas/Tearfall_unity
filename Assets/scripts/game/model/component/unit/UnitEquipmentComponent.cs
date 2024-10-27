using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.component.unit {
// contains slots for wearable and holdable items (clothes and tools)
public struct UnitEquipmentComponent {
    public Dictionary<string, WearEquipmentSlot> slots; // all slots of a creature (for wear)
    public Dictionary<string, GrabEquipmentSlot> grabSlots; // slots for tools (subset of all slots)
    public readonly Dictionary<string, List<EcsEntity>> coverageItems;
    // TODO add coverage map with protection values
    public HashSet<EcsEntity> items; // items in worn containers and in hands
    public bool desiredSlotsFilled;
    public EcsEntity hauledItem;

    public bool toolWithActionEquipped(String action) {
        return grabSlots.Values
            .Where(slot => slot.isToolGrabbed())
            .Select(slot => slot.item.Get<ItemToolComponent>())
            .Count(tool => tool.action == action) > 0;
    }

    public WearEquipmentSlot findWearSlotWithItem(EcsEntity item) {
        return slots.Values
            .Where(slot => slot.item == item || slot.armorItem == item || slot.overItem == item)
            .DefaultIfEmpty(null).First();
    }
    
    public GrabEquipmentSlot findGrabSlotWithItem(EcsEntity item) {
        return grabSlots.Values
            .Where(slot => slot.item == item)
            .DefaultIfEmpty(null).First();
    }
    
    public GrabEquipmentSlot getSlotWithoutCooldown() {
        foreach (var slot in grabSlots.Values) {
            if (slot.cooldown <= 0) {
                if (slot.item == EcsEntity.Null || slot.item.Has<ItemWeaponComponent>()) { // empty slots for unarmed attacks
                    return slot;
                }
            }
        }
        return null;
    }
    
    public EcsEntity getEquippedShieldWithoutCooldown() {
        foreach (var slot in grabSlots.Values) {
            if (slot.item != EcsEntity.Null && slot.item.Has<ItemShieldComponent>()) {
                if (slot.item.take<ItemShieldComponent>().cooldown <= 0) {
                    return slot.item;
                }
            }
        }
        return EcsEntity.Null;
    }
    
    // returns ranged weapon with corresponding ammunition available
    public EcsEntity getRangedWeapon() {
        return EcsEntity.Null; // TODO 
    }

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
    public EcsEntity item = EcsEntity.Null; // wear item
    public EcsEntity armorItem = EcsEntity.Null;
    public EcsEntity overItem = EcsEntity.Null;
    
    public WearEquipmentSlot(string name, List<string> limbs) : base(name, limbs) { }
}

// for holding items like tools and weapons
public class GrabEquipmentSlot : AbstractEquipmentSlot {
    public EcsEntity item = EcsEntity.Null;
    public float cooldown;

    public GrabEquipmentSlot(string name, List<string> limbs) : base(name, limbs) { }

    public bool isGrabFree() {
        return item == EcsEntity.Null;
    }

    public bool isToolGrabbed() {
        return item != EcsEntity.Null && item.Has<ItemToolComponent>();
    }
}
}