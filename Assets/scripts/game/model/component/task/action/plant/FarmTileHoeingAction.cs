using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.util;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.item;
using util.lang.extension;
using static types.action.ActionConditionStatusEnum;

namespace game.model.component.task.action.plant {
    // actually hoes one tile of a farm. See FarmHoeingAction
    // also destroys plant and substrate on tile
    public class FarmTileHoeingAction : Action {
        private const string toolActionName = "hoe";

        public FarmTileHoeingAction(Vector3Int tile) : base(new PositionActionTarget(tile, ActionTargetTypeEnum.NEAR)) {
            startCondition = () => {
                if (!performer.take<UnitEquipmentComponent>().toolWithActionEquipped(toolActionName)) return tryCreateEquippingAction();
                return ZoneUtils.tileUnhoed(tile, model) ? OK : FAIL;
            };
            maxProgress = 200;
            onFinish = () => hoeTile(tile);
        }

        private ActionConditionStatusEnum tryCreateEquippingAction() {
            ItemSelector toolItemSelector = new ToolWithActionItemSelector(toolActionName);
            EcsEntity item = model.itemContainer.util.findFreeReachableItemBySelector(toolItemSelector, performer.pos());
            if (item == EcsEntity.Null) return FAIL;
            addPreAction(new EquipToolItemAction(item));
            return NEW;
        }

        private void hoeTile(Vector3Int tile) {
            model.farmContainer.addFarm(tile);
            model.plantContainer.removePlant(tile, true);
            model.localMap.substrateMap.remove(tile);
        }
    }
}