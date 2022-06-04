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
            if (container.onMapItems.all.Count < 0) return EcsEntity.Null;
            return container.onMapItems.all
                .Where(item => map.passageMap.inSameArea(pos, item.pos()))
                .Where(selector.checkItem)
                .DefaultIfEmpty(EcsEntity.Null)
                .Aggregate((cur, item) => cur == EcsEntity.Null || (distanceToItem(item, pos) < distanceToItem(cur, pos))
                    ? item
                    : cur); // select nearest]
        }

        public Dictionary<string, MultiValueDictionary<int, EcsEntity>> findForConstruction(ConstructionType type) {
            Dictionary<string, MultiValueDictionary<int, EcsEntity>> resultMap = new();
            foreach (string materialOption in type.materials) {
                string[] args = materialOption.Split("/");
                string typeName = args[0];
                int requiredAmount = int.Parse(args[1]);
                MultiValueDictionary<int, EcsEntity> map = container.availableItemsManager.findByType(typeName);
                map.removeByPredicate(entities => entities.Count < requiredAmount);
                resultMap.Add(materialOption, map);
            }
            return resultMap;
        }

        // checks if there are at least one option to build construction
        public bool enoughForConstructionType(ConstructionType type) {
            foreach (string materialOption in type.materials) {
                string[] args = materialOption.Split("/");
                string typeName = args[0];
                int requiredAmount = int.Parse(args[1]);
                if (container.availableItemsManager.findByType(typeName).Values
                    .Any(items => items.Count > requiredAmount)) return true;
            }
            return false;
        }

        private float distanceToItem(EcsEntity item, Vector3Int position) {
            return Vector3Int.Distance(item.pos(), position);
        }
    }
}