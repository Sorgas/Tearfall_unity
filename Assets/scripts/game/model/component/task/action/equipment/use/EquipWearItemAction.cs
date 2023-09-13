using System.Linq;
using game.model.component.item;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util;
using util.lang.extension;
using static game.model.util.EquipmentSlotUtil;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.equipment.use {
public class EquipWearItemAction : PutItemToDestinationAction {
    public EquipWearItemAction(EcsEntity targetItem) : base(new SelfActionTarget(), targetItem) {
        name = $"equip {targetItem.name()}";

        startCondition = () => {
            if (!validate()) return FAIL;
            lockEntity(item);
            return verifyItemHauled(); // can create obtain item action
        };

        onFinish = () => {
            ItemWearComponent wear = item.takeRef<ItemWearComponent>();
            WearEquipmentSlot targetSlot = equipment.slots[wear.slot];
            dropCurrentItem(targetSlot, wear.layer);
            if (validate()) {
                addItemToSlot();
            } else {
                model.itemContainer.transition.fromUnitToGround(item, performer, performer.pos());
            }
            equipment.hauledItem = EcsEntity.Null;
            updateWearNeed();
        };
    }

    // validates that item is wear and unit has correct slot
    public override bool validate() {
        if (!item.Has<ItemWearComponent>()) {
            Debug.LogWarning($"{name}: target item {item.name()} is not wear");
            return false;
        }
        string layer = item.take<ItemWearComponent>().layer;
        if (!validLayers.Contains(layer)) {
            Debug.LogWarning($"{name}: target item {item.name()} has invalid slot layer {layer}");
            return false;
        }
        ItemWearComponent wear = item.take<ItemWearComponent>();
        if (!equipment.slots.ContainsKey(wear.slot)) {
            Debug.LogWarning($"{name}: unit has no correct slot ({wear.slot}) to equip {item.name()}");
        }
        return base.validate() && equipment.slots[wear.slot] != null;
    }

    private void dropCurrentItem(WearEquipmentSlot slot, string layer) {
        EcsEntity previousItem = removeItemFromSlot(slot, layer);
        if (previousItem != EcsEntity.Null) {
            model.itemContainer.transition.fromUnitToGround(previousItem, performer, performer.pos());
        }
    }

    // drops component, so it can be recalculated 
    private void updateWearNeed() => performer.Del<UnitCalculatedWearNeedComponent>();

    private void addItemToSlot() {
        ItemWearComponent wear = item.takeRef<ItemWearComponent>();
        WearEquipmentSlot targetSlot = equipment.slots[wear.slot];
        if (wear.layer == "wear") {
            targetSlot.item = item;
        } else if (wear.layer == "armor") {
            targetSlot.armorItem = item;
        } else if (wear.layer == "over") {
            targetSlot.overItem = item;
        } else {
            throw new GameException("Wear item " + item.name() + " has invalid layer value: " + wear.layer);
        }
    }
}
}