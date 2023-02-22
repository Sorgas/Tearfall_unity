using game.model.util;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
    // gives free cell of stockpile as target position
    public class StockpileActionTarget : ActionTarget {
        private readonly EcsEntity stockpile;
        private Vector3Int? position = Vector3Int.back;
        
        public StockpileActionTarget(EcsEntity stockpile) : base(ActionTargetTypeEnum.EXACT) {
            this.stockpile = stockpile;
        }

        public override Vector3Int? Pos => position != Vector3Int.back ? position : null;

        public Vector3Int lookupFreeTile() {
            position = ZoneUtils.findFreeStockpileCells(stockpile.take<ZoneComponent>(), stockpile.take<StockpileComponent>(), GameModel.get().currentLocalModel);
            return position.Value;
        }
    }
}