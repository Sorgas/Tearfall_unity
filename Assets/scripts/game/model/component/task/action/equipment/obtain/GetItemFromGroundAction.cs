using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static enums.action.ActionConditionStatusEnum;

namespace game.model.component.task.action.equipment.obtain {
    /**
    * Action for picking up item. Performer should have UnitEquipmentComponent.
    * Item is put to special hauledItem field.
    * Item should be on the ground, (see ObtainItemAction).
    *
    * @author Alexander on 12.01.2019.
    */
    public class GetItemFromGroundAction : EquipmentAction {

        public GetItemFromGroundAction(EcsEntity item) : base(new ItemActionTarget(item), item) {

            startCondition = () => {
                UnitEquipmentComponent equipment = base.equipment();
                if (equipment.hauledItem != null) {
                    return addPreAction(new PutItemToPositionAction(equipment.hauledItem.Value, performer().Get<PositionComponent>().position));
                }
                return !validate() ? FAIL : OK;
            };

            onStart = () => maxProgress = 20;

            onFinish = () => { // add item to unit
                // TODO remove item from map
                // container
                equipment().hauledItem = item;
            };
        }

        protected new bool validate() {
            LocalMap map = GameModel.localMap;
            if (item.hasPos()) {
                Vector3Int itemPosition = item.pos();
                return base.validate()
                       && container.itemMap.ContainsKey(itemPosition)
                       && container.itemMap[itemPosition].Contains(item)
                       && map.passageMap.inSameArea(itemPosition, performer().pos());
            }
            return false;
        }
    }
}