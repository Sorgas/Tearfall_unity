using System;
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
        private const string TOOL_ACTION_NAME = "hoe";
        private EcsEntity zone;

        public FarmTileHoeingAction(Vector3Int tile, EcsEntity zone) : base(new PositionActionTarget(tile, ActionTargetTypeEnum.ANY)) {
            name = "tile hoeing action";
            this.zone = zone;
            startCondition = () => {
                if (!performer.take<UnitEquipmentComponent>().toolWithActionEquipped(TOOL_ACTION_NAME)) return tryCreateEquippingAction();
                if (!ZoneUtils.tileUnhoed(tile, model)) return FAIL;
                lockZoneTile(zone, tile);
                return OK;
            };
            maxProgress = 100;
            onFinish = () => {
                hoeTile(tile);
                unlockZoneTile(zone, tile);
            };
        }

        private ActionConditionStatusEnum tryCreateEquippingAction() {
            ItemSelector toolItemSelector = new ToolWithActionItemSelector(TOOL_ACTION_NAME);
            EcsEntity item = model.itemContainer.util.findFreeReachableItemBySelector(toolItemSelector, performer.pos());
            if (item == EcsEntity.Null) return FAIL;
            addPreAction(new EquipToolItemAction(item));
            return NEW;
        }

        private void hoeTile(Vector3Int tile) {
            model.farmContainer.addFarm(tile); // triggers tile update 
            model.plantContainer.removePlant(tile, true);
            model.localMap.substrateMap.remove(tile);
        }
    }
}