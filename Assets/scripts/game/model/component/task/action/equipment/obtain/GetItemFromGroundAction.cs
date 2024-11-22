﻿using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.equipment.obtain {
/**
* Action for picking up item. Performer should have UnitEquipmentComponent. Locks item.
* Item is put to special hauledItem field.
* Item should be on the ground, (see ObtainItemAction).
* Item should not be locked.
*
* @author Alexander on 12.01.2019.
*/
public class GetItemFromGroundAction : ItemAction {
    public GetItemFromGroundAction(EcsEntity item) : base(new ItemActionTarget(item), item) {
        name = $"get {item.name()} from ground action";
        maxProgress = 20;
        startCondition = () => {
            if (!validate()) return FAIL;
            lockEntity(item);
            UnitEquipmentComponent equipment = this.equipment;
            if (equipment.hauledItem != EcsEntity.Null) {
                return addPreAction(new PutItemToPositionAction(equipment.hauledItem, performer.pos()));
            }
            return OK;
        };

        onFinish = () => { // add item to unit
            model.itemContainer.transition.fromGroundToUnit(item, performer);
            equipment.hauledItem = item;
        };
    }

    protected new bool validate() {
        return base.validate()
               && item.hasPos()
               && model.localMap.passageMap.inSameArea(item.pos(), performer.pos());
    }
}
}