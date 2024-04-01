using game.model.component.item;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.equipment.obtain {
// TODO add handling of cases when item got moved from-to container after creation of pre-actions
public class ObtainItemAction : ItemAction {
    public ObtainItemAction(EcsEntity item) : base(new SelfActionTarget(), item) {
        name = "obtain item action";
        startCondition = () => {
            if (!validate()) return FAIL;
            if (equipment.hauledItem == item) return OK;
            if (item.Has<ItemContainedComponent>()) return addPreAction(new GetItemFromContainerAction(item));
            if (item.Has<PositionComponent>()) return addPreAction(new GetItemFromGroundAction(item)); // pickup from ground
            if (model.itemContainer.equipped.itemEquipped(item)) {
                if (model.itemContainer.equipped.equippedItems[item] == performer) {
                    return addPreAction(new GetItemFromEquipmentAction(item));
                } else {
                    Debug.Log($"Item {item.name()} equipped on another unit.");
                    return FAIL;
                }
            }
            return FAIL; // item is not registered in container or equipped on another unit
        };
    }
}
}