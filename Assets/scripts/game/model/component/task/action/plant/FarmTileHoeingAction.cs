using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.item;
using util.lang.extension;

namespace game.model.component.task.action.plant {
    // actually hoes one tile of a farm. See FarmHoeingAction
    public class FarmTileHoeingAction : Action {
        private const string toolActionName = "hoe";

        public FarmTileHoeingAction(Vector3Int tile) : base(new PositionActionTarget(tile, ActionTargetTypeEnum.NEAR)) {
            startCondition = () => {
                if (!performer.take<UnitEquipmentComponent>().toolWithActionEquipped(toolActionName)) return tryCreateEquippingAction();
                return ActionConditionStatusEnum.OK;
            };
            
            onStart = () => maxProgress = 200;

            onFinish = () => {
                model.localMap.blockType.set(tile, BlockTypes.FARM);
            };
        }

        private ActionConditionStatusEnum tryCreateEquippingAction() {
            ItemSelector toolItemSelector = new ToolWithActionItemSelector("hoe");
            EcsEntity item = model.itemContainer.util.findFreeReachableItemBySelector(toolItemSelector, performer.pos());
            if (item != EcsEntity.Null) {
                addPreAction(new EquipToolItemAction(item));
                return ActionConditionStatusEnum.NEW;
            }
            return ActionConditionStatusEnum.FAIL;
        }
    }
}