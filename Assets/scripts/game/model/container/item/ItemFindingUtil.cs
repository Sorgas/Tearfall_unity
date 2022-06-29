using System.Collections.Generic;
using System.Linq;
using game.model.localmap;
using Leopotam.Ecs;
using types.building;
using UnityEngine;
using util.item;
using util.lang;
using util.lang.extension;

namespace game.model.container.item {
    public class ItemFindingUtil {
        private ItemContainer container;

        public ItemFindingUtil(ItemContainer container) {
            this.container = container;
        }

        public EcsEntity findFreeReachableItemBySelector(ItemSelector selector, Vector3Int pos) {
            LocalMap map = GameModel.localMap;
            // get items and positions
            // TODO add items in containers
            if (container.onMap.all.Count < 0) return EcsEntity.Null;
            return container.onMap.all
                .Where(item => map.passageMap.inSameArea(pos, item.pos()))
                .Where(selector.checkItem)
                .DefaultIfEmpty(EcsEntity.Null)
                .Aggregate((cur, item) => cur == EcsEntity.Null || (distanceToItem(item, pos) < distanceToItem(cur, pos))
                    ? item
                    : cur); // select nearest
        }

        // returns variant -> material -> list of items 
        public Dictionary<BuildingVariant, MultiValueDictionary<int, EcsEntity>> findForBuildingVariants(BuildingVariant[] variants) {
            Dictionary<BuildingVariant, MultiValueDictionary<int, EcsEntity>> resultMap = new();
            foreach (BuildingVariant variant in variants) {
                MultiValueDictionary<int, EcsEntity> map = container.availableItemsManager.findByType(variant.itemType);
                map.removeByPredicate(entities => entities.Count < variant.amount);
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

        private float distanceToItem(EcsEntity item, Vector3Int position) {
            return Vector3Int.Distance(item.pos(), position);
        }
    }
}