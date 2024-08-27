using System.Linq;
using game.model.component.item;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.component.task.action.equipment.use {
/**
     * Action for equipping tool or weapon items into appropriate slot.
     * Item can be equipped, if creature has enough grab slots.
     * All grabbed items from target slots will be dropped.
     *
     * Slot names are 'hardcoded' for humanoid body type.
     */
public class EquipToolItemAction : PutItemToDestinationAction {
    private const string RIGHT = "right hand";
    private const string LEFT = "left hand";

    public EquipToolItemAction(EcsEntity item) : base(new SelfActionTarget(), item) {
        debug = true;
        name = "equip tool action";
        onFinish = () => {
            equipItem(item);
            equipment.hauledItem = EcsEntity.Null;
        };
    }

    // Equips item to appropriate slot. If slot is occupied, drops occupying item
    private void equipItem(EcsEntity item) {
        string gripType = item.take<ItemGripComponent>().type;
        GrabEquipmentSlot rightSlot = equipment.grabSlots[RIGHT];
        GrabEquipmentSlot leftSlot = equipment.grabSlots[LEFT];
        if ("main".Equals(gripType)) {
            if (rightSlot.item != EcsEntity.Null) {
                string equippedItemGripType = rightSlot.item.take<ItemGripComponent>().type;
                if (equippedItemGripType.Equals("any") && leftSlot.isGrabFree()) {
                    leftSlot.item = rightSlot.item; // move item to another free slot
                } else {
                    dropGrabbedItemFromSlot(rightSlot);
                }
            }
            equipItem(item, rightSlot);
        }
        if ("off".Equals(gripType)) {
            if (leftSlot.item != EcsEntity.Null) {
                string equippedItemGripType = leftSlot.item.take<ItemGripComponent>().type;
                if (equippedItemGripType.Equals("any") && rightSlot.isGrabFree()) {
                    rightSlot.item = leftSlot.item; // move item to another free slot
                } else {
                    dropGrabbedItemFromSlot(leftSlot);
                }
            }
            equipItem(item, leftSlot);
        } else if ("two".Equals(gripType)) {
            if (!rightSlot.isGrabFree()) dropGrabbedItemFromSlot(rightSlot);
            if (!leftSlot.isGrabFree()) dropGrabbedItemFromSlot(leftSlot);
            rightSlot.item = item;
            leftSlot.item = item;
            log($"{item.name()} equipped to both hands");
        } else if ("any".Equals(gripType)) {
            if (rightSlot.isGrabFree()) {
                equipItem(item, rightSlot);
            } else if (leftSlot.isGrabFree()) {
                equipItem(item, leftSlot);
            } else { // both slots occupied
                dropGrabbedItemFromSlot(rightSlot);
                equipItem(item, rightSlot);
            }
        }
    }

    // ensures there are no tools in grab slots and at least one slot is empty
    private void prepareSlotForEquippingTool() {
        // remove all other tools and drop to map
        foreach (var grabEquipmentSlot in equipment.grabSlots.Values.Where(slot => slot.isToolGrabbed())) {
            dropGrabbedItemFromSlot(grabEquipmentSlot);
        }
        // remove items from one slot, if all are occupied by items
        if (equipment.grabSlots.Values.All(slot => !slot.isGrabFree())) {
            dropGrabbedItemFromSlot(equipment.grabSlots.Values.First());
        }
    }

    // Drops item from grab slot to ground and clears slot. Supports two-handed items.
    private void dropGrabbedItemFromSlot(GrabEquipmentSlot slot) {
        GrabEquipmentSlot rightSlot = equipment.grabSlots[RIGHT];
        GrabEquipmentSlot leftSlot = equipment.grabSlots[LEFT];
        if (rightSlot.item == leftSlot.item) { // two-handed item equipped
            rightSlot.item = EcsEntity.Null;
            leftSlot.item = EcsEntity.Null;
            log($"{item.name()} dropped from both hands");
        } else {
            slot.item = EcsEntity.Null;
            log($"{item.name()} dropped from {slot.name}");
        }
        EcsEntity droppedItem = slot.item;
        container.transition.fromUnitToGround(droppedItem, performer);
    }

    // Assigns item to slot and does logging. Supports only one-handed items.
    private void equipItem(EcsEntity item, GrabEquipmentSlot slot) {
        slot.item = item;
        log($"{item.name()} equipped to {slot.name}");
    }

    public override bool validate() {
        return base.validate() && item.Has<ItemGripComponent>() && equipment.grabSlots.Count > 0;
    }
}
}