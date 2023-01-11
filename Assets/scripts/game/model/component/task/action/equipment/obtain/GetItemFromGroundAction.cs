using game.model.component.item;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionConditionStatusEnum;

namespace game.model.component.task.action.equipment.obtain {
    /**
    * Action for picking up item. Performer should have UnitEquipmentComponent. Locks item.
    * Item is put to special hauledItem field.
    * Item should be on the ground, (see ObtainItemAction).
    * Item should not be locked.
    *
    * @author Alexander on 12.01.2019.
    */
    public class GetItemFromGroundAction : EquipmentAction {

        public GetItemFromGroundAction(EcsEntity item) : base(new ItemActionTarget(item), item) {
            name = "get item from ground action";
            startCondition = () => {
                if (!validate()) return FAIL;
                lockEntity(item);
                UnitEquipmentComponent equipment = base.equipment();
                if (equipment.hauledItem != EcsEntity.Null) {
                    return addPreAction(new PutItemToPositionAction(equipment.hauledItem, performer.pos()));
                }
                return OK;
            };

            onStart = () => maxProgress = 20;

            onFinish = () => { // add item to unit
                model.itemContainer.transition.fromGroundToUnit(item, performer);
                equipment().hauledItem = item;
            };
        }

        protected new bool validate() {
            LocalMap map = model.localMap;
            if(!itemCanBeLocked(item) || !item.hasPos()) return false;
            Vector3Int itemPosition = item.pos();
            return base.validate()
                   && container.onMap.itemsOnMap.ContainsKey(itemPosition)
                   && container.onMap.itemsOnMap[itemPosition].Contains(item)
                   && map.passageMap.inSameArea(itemPosition, performer.pos());
        }
    }
}