using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.component.task.order;
using game.model.localmap;
using game.model.util;
using Leopotam.Ecs;
using types.building;
using UnityEngine;
using util.item;
using util.lang;
using util.lang.extension;

namespace game.model.container.item.finding {
// General utility class for searching items
    public class ItemFindingUtil : AbstractItemFindingUtil {
        // TODO rewrite stockpile method to use item selectors. selector should be stored in stockpile component and updated from stockpile config menu.
        public ItemFindingUtil(LocalModel model, ItemContainer container) : base(model, container) { }

        public EcsEntity findItemBySelector(ItemSelector selector, Vector3Int pos) {
            return container.availableItemsManager.getAll()
                .Where(item => !item.Has<LockedComponent>())
                .Where(selector.checkItem)
                .firstOrDefault(item => model.localMap.passageMap.inSameArea(pos, container.getItemAccessPosition(item)), EcsEntity.Null); // select nearest
        }

        // returns nearest item matching itemSelector, available from position and not locked to task
        public EcsEntity findNearestItemBySelector(ItemSelector selector, Vector3Int pos) {
            return container.availableItemsManager.getAll()
                .Where(item => !item.Has<LockedComponent>())
                .Where(selector.checkItem)
                .ToDictionary(item => item, item => container.getItemAccessPosition(item))
                .Where(pair => model.localMap.passageMap.inSameArea(pos, pair.Value))
                .DefaultIfEmpty()
                .Aggregate((cur, item) =>
                    cur.Key == EcsEntity.Null || (fastDistance(item.Value, pos) < fastDistance(cur.Value, pos)) ? item : cur)
                .Key; // select nearest
        }

        public EcsEntity findForStockpile(StockpileComponent stockpile, List<Vector3Int> zonePositions, Vector3Int position) {
            List<EcsEntity> list = container.availableItemsManager.getAll()
                .Where(item => !item.Has<LockedComponent>()) // item not locked by another task
                .Where(item => !zonePositions.Contains(container.getItemAccessPosition(item))) // item not in stockpile already
                .Where(item => ZoneUtils.itemAllowedInStockpile(stockpile, item.take<ItemComponent>())) // item allowed for stockpile
                .ToList();
            return list.Count == 0 ? EcsEntity.Null : selectNearest(list, position);
        }
        
        private EcsEntity selectNearest(List<EcsEntity> items, Vector3Int position) {
            float minDistance = -1;
            EcsEntity result = EcsEntity.Null;
            foreach (EcsEntity item in items) {
                float distance = fastDistanceToItem(item, position);
                if (distance == 0) return item;
                if (distance < minDistance || minDistance < 0) {
                    result = item;
                    minDistance = distance;
                }
            }
            return result;
        }
        
        private void log(string message) => Debug.Log("[ItemFindingUtil]: " + message);
    }
}