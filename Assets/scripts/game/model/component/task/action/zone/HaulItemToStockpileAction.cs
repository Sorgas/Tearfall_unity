using game.model.component.task.action.equipment.obtain;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionConditionStatusEnum;

namespace game.model.component.task.action.zone {
    // TODO add containers usage
    // when assigned, searches item that can be brought to stockpile. 
    public class HaulItemToStockpileAction : Action {
        private StockpileActionTarget actionTarget;
        private EcsEntity item = EcsEntity.Null;
        private EcsEntity stockpile;
        
        // TODO use zone as target
        public HaulItemToStockpileAction(StockpileActionTarget target) : base(target) {
            actionTarget = target;
            startCondition = () => {
                if (!checkFreeTile()) return FAIL;
                if (item == EcsEntity.Null && !findItem()) return FAIL;
                if (performer.take<UnitEquipmentComponent>().hauledItem != item) return addPreAction(new ObtainItemAction(item));
                return OK;
            };

            onFinish = () => { };
        }
        
        // TODO optimize with flag in StockpileComponent
        private bool checkFreeTile() {
            Vector3Int tile = actionTarget.lookupFreeTile();
            return tile != Vector3Int.back;
        }
        
        private bool findItem() {
            item = model.itemContainer.util.findForStockpile(stockpile.take<StockpileComponent>().map, stockpile.pos());
            return item != EcsEntity.Null;
        }
    }
}