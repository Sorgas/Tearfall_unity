using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.equipment.obtain {
// Obtains item form performer's equipment and moves to performer's buffer
public class GetItemFromEquipmentAction : ItemAction {
    public GetItemFromEquipmentAction(EcsEntity item) : base(new SelfActionTarget(), item) {
        startCondition = () => {
            if (!model.itemContainer.equipped.itemEquipped(item)
                || model.itemContainer.equipped.equippedItems[item] != performer
                || (equipment.findWearSlotWithItem(item) == null && equipment.findGrabSlotWithItem(item) == null)) {
                Debug.Log($"Inconsistency detected for equipped item {item.name()} for unit {performer.name()}");
                return FAIL;
            }
            if (equipment.hauledItem != EcsEntity.Null)
                return addPreAction(new PutItemToPositionAction(equipment.hauledItem, performer.pos()));
            return OK;
        };

        onFinish = () => {
            if (removeItemFromEquipment()) {
                performer.Del<UnitCalculatedWearNeedComponent>();
                equipment.hauledItem = item;
            } else {
                Debug.LogError($"Item {item.name()} not found in {performer.name()} equipment.");
            }
        };
    }

    private bool removeItemFromEquipment() {
        WearEquipmentSlot wearSlot = equipment.findWearSlotWithItem(item);
        if (wearSlot != null) {
            if (wearSlot.item == item) {
                wearSlot.item = EcsEntity.Null;
                return true;
            }
            if (wearSlot.armorItem == item) {
                wearSlot.armorItem = EcsEntity.Null;
                return true;
            }
            if (wearSlot.overItem == item) {
                wearSlot.overItem = EcsEntity.Null;
                return true;
            }
            return false;
        }
        GrabEquipmentSlot grabSlot = equipment.findGrabSlotWithItem(item);
        if (grabSlot != null) {
            if (grabSlot.item != item) return false;
            grabSlot.item = EcsEntity.Null;
            return true;
        }
        return false;
    }
}
}