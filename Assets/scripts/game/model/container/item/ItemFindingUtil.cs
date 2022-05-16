using System.Linq;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.item;
using util.lang.extension;

namespace game.model.container.item {
    public class ItemFindingUtil {
        
        public EcsEntity findFreeReachableItemBySelector(ItemSelector selector, Vector3Int pos) {
            LocalMap map = GameModel.localMap;
            // get items and positions
            // TODO add items in containers
            if(GameModel.get().itemContainer.onMapItems.all.Count < 0) return EcsEntity.Null;
            return GameModel.get().itemContainer.onMapItems.all
                .Where(item => map.passageMap.inSameArea(pos, item.pos()))
                .Where(selector.checkItem)
                .DefaultIfEmpty(EcsEntity.Null)
                .Aggregate((cur, item) => cur == EcsEntity.Null || (distanceToItem(item, pos) < distanceToItem(cur, pos))
                    ? item
                    : cur); // select nearest]
        }

        private float distanceToItem(EcsEntity item, Vector3Int position) {
            return Vector3Int.Distance(item.pos(), position);
        }
    }
}