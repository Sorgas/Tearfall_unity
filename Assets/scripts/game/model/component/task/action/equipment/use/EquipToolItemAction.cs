﻿using System.Linq;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.equipment.use {
    /**
    * Action for equipping tool items into appropriate slot.
    * Tool can be equipped, if creature has one grab slot.
    * All other tools in grab slots are unequipped.
    * TODO make two handed tools, probably making main-hand and off-hand, and adding comprehensive requirements to tools.
    */
    public class EquipToolItemAction : PutItemToDestinationAction {
        public EquipToolItemAction(EcsEntity item) : base(new SelfActionTarget(), item) {
            onFinish = () => {
                prepareSlotForEquippingTool();
                equipment().grabSlots.Values.First(slot => slot.isGrabFree()).grabbedItem = item;
                equipment().hauledItem = null;
                container.equippedItems.addItemToUnit(item, performer());
                Debug.Log(item + " equipped as tool");
                //TODO select one or more hands to maintain main/off hand logic
            };
        }

        // ensures there are no tools in grab slots and at least one slot is empty
        private void prepareSlotForEquippingTool() {
            // remove all other tools and drop to map
            foreach (var grabEquipmentSlot in equipment().grabSlots.Values.Where(slot => slot.isToolGrabbed())) {
                dropGrabbedItemFromSlot(grabEquipmentSlot);
            }
            // remove items from one slot, if all are occupied by items
            if (equipment().grabSlots.Values.All(slot => !slot.isGrabFree())) {
                dropGrabbedItemFromSlot(equipment().grabSlots.Values.First());
            }
        }

        // drops item from grab slot to ground and clears slot
        private void dropGrabbedItemFromSlot(GrabEquipmentSlot slot) {
            EcsEntity droppedItem = slot.grabbedItem.Value;
            slot.grabbedItem = null;
            container.equippedItems.removeItemFromUnit(droppedItem, performer());
            container.onMapItems.putItemToMap(droppedItem, performer().pos());
        }

        protected new bool validate() {
            return base.validate()
                   // TODO validate that item is tool
                   // && item.type.tool != null
                   && equipment().grabSlots.Count > 0;
        }
    }
}