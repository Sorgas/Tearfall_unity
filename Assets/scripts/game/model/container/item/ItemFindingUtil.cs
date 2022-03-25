using System.Linq;
using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.item;
using util.lang.extension;

namespace game.model.container.item {
    public class ItemFindingUtil {
        
        public EcsEntity? findFreeReachableItemBySelector(ItemSelector selector, Vector3Int position) {
            LocalMap map = GameModel.localMap;
            // get items and positions
            // TODO add items in containers
            return GameModel.get().itemContainer.onMapItems.all
                .Where(selector.checkItem)
                .Where(item => map.passageMap.inSameArea(position, item.pos()))
                // .DefaultIfEmpty()// filter reachability
                .Aggregate((current, item) => current == null || (distanceToItem(item, position) < distanceToItem(current, position))
                    ? item
                    : current); // select nearest
        }

        private float distanceToItem(EcsEntity item, Vector3Int position) {
            return Vector3Int.Distance(item.pos(), position);
        }
    }
}