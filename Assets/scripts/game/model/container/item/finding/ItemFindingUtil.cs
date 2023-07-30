using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.localmap;
using game.model.util;
using Leopotam.Ecs;
using types.building;
using UnityEngine;
using util.item;
using util.lang;
using util.lang.extension;

namespace game.model.container.item.finding {
    public class ItemFindingUtil : AbstractItemFindingUtil {
        // TODO rewrite stockpile method to use item
        // selectors. selector should be stored in stockpile component and updated from stockpile config menu.
        public ItemFindingUtil(LocalModel model, ItemContainer container) : base(model, container) { }

        // returns item matching itemSelector and available from position
        public EcsEntity findFreeReachableItemBySelector(ItemSelector selector, Vector3Int pos) {
            LocalMap map = model.localMap;
            // get items and positions
            // TODO add items in containers
            return container.availableItemsManager.getAll()
                .Where(item => !item.Has<LockedComponent>())
                .Where(item => map.passageMap.inSameArea(pos, container.getItemAccessPosition(item)))
                .Where(selector.checkItem)
                .DefaultIfEmpty(EcsEntity.Null)
                .Aggregate((cur, item) => cur == EcsEntity.Null || (fastDistance(item, pos) < fastDistance(cur, pos))
                    ? item
                    : cur); // select nearest
        }

        // returns variant -> materialId -> list of items 
        public Dictionary<BuildingVariant, MultiValueDictionary<int, EcsEntity>> findForBuildingVariants(BuildingVariant[] variants) {
            Dictionary<BuildingVariant, MultiValueDictionary<int, EcsEntity>> resultMap = new();
            foreach (BuildingVariant variant in variants) {
                MultiValueDictionary<int, EcsEntity> map = container.availableItemsManager.findByType(variant.itemType);
                resultMap.Add(variant, map);
            }
            return resultMap;
        }

        // checks if there are at least one option to build construction
        public bool enoughForBuilding(BuildingVariant[] variants) {
            foreach (BuildingVariant variant in variants) {
                if (container.availableItemsManager.findByType(variant.itemType).Values
                    .Any(items => items.Count > variant.amount)) return true;
            }
            return false;
        }

        public EcsEntity findFoodItem(Vector3Int position) {
            List<EcsEntity> list = container.availableItemsManager.getAll()
                .Where(item => item.Has<ItemFoodComponent>())
                .ToList();
            if (list.Count == 0) return EcsEntity.Null;
            return selectNearest(list, position);
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
                float distance = fastDistance(item, position);
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