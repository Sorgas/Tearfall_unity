using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.util;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.zone {
    // TODO add containers usage (store solid items to bins and barrels, grains to sacks)
    // when assigned, searches item that can be brought to stockpile, locks item and tile, then creates put to position action.
    // Brings only one item and then ends
    public class StoreItemToStockpileAction : ZoneAction {
        private readonly ZoneActionTarget actionTarget;
        private Vector3Int targetTile = Vector3Int.back;
        private EcsEntity targetItem = EcsEntity.Null;
        private bool putActionCreated = false; // to check that put action will be created only once
        
        public StoreItemToStockpileAction(EcsEntity zone) : base(zone, ActionTargetTypeEnum.ANY) {
            name = "haul item to " + zone.name();
            actionTarget = (ZoneActionTarget)target;
            startCondition = () => {
                if (targetTile == Vector3Int.back && !findFreeTile()) return FAIL;
                if (targetItem == EcsEntity.Null && !findItem()) return FAIL;
                if (!putActionCreated) {
                    lockZoneTile(zone, targetTile);
                    lockEntity(targetItem);
                    putActionCreated = true;
                    return addPreAction(new PutItemToPositionAction(targetItem, targetTile));
                }
                return OK; // put action was created and was not failed
            };
        }
        
        private bool findFreeTile() {
            targetTile = ZoneUtils.findFreeStockpileTile(zone, task, model);
            if (targetTile != Vector3Int.back) actionTarget.tile = targetTile; 
            return targetTile != Vector3Int.back;
        }

        private bool findItem() {
            targetItem = model.itemContainer.findingUtil.findForStockpile(zone.take<StockpileComponent>(), zone.take<ZoneComponent>().tiles, targetTile);
            return targetItem != EcsEntity.Null;
        }

        public override bool validate() => true;
    }
}