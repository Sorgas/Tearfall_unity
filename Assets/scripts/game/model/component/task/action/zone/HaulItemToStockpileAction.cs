using game.model.component.task.action.equipment;
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
    public class HaulItemToStockpileAction : EquipmentAction {
        private StockpileActionTarget actionTarget;
        private EcsEntity stockpile;
        private Vector3Int targetTile;

        // TODO use zone as target
        public HaulItemToStockpileAction(StockpileActionTarget target) : base(target, EcsEntity.Null) {
            name = "haul item to " + target.stockpile.name();
            actionTarget = target;
            stockpile = target.stockpile;
            startCondition = () => {
                if (!checkFreeTile()) return FAIL;
                if (item == EcsEntity.Null && !findItem()) return FAIL;
                if (performer.take<UnitEquipmentComponent>().hauledItem != item) return addPreAction(new ObtainItemAction(item));
                return OK;
            };

            onFinish = () => {
                ref UnitEquipmentComponent equipment = ref this.equipment();
                equipment.hauledItem = EcsEntity.Null;
                model.itemContainer.transition.fromUnitToGround(item, performer, targetTile);
            };
        }

        // TODO optimize with flag in StockpileComponent
        private bool checkFreeTile() {
            targetTile = actionTarget.lookupFreeTile();
            return targetTile != Vector3Int.back;
        }

        private bool findItem() {
            item = model.itemContainer.util.findForStockpile(stockpile.take<StockpileComponent>(), stockpile.take<ZoneComponent>().tiles,
                stockpile.take<ZoneComponent>().tiles[0]);
            return item != EcsEntity.Null;
        }
    }
}