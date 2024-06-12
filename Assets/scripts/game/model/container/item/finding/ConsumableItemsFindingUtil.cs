using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using game.model.component.item;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;
using util.lang.extension;

namespace game.model.container.item.finding {
public class ConsumableItemsFindingUtil : AbstractItemFindingUtil {
    public ConsumableItemsFindingUtil(LocalModel model, ItemContainer container) : base(model, container) { }
    
    public EcsEntity findFoodItem(Vector3Int position, int minimumFoodQuality) {
        List<EcsEntity> list = container.availableItemsManager.getAll()
            .Where(item => item.Has<ItemFoodComponent>())
            .Where(item => item.take<ItemFoodComponent>().foodQuality <= minimumFoodQuality)
            .ToList();
        if (list.Count == 0) return EcsEntity.Null;
        return selectBestNearestItem(list, position);
    }

    // TODO rewrite to o(n)
    private EcsEntity selectBestNearestItem(List<EcsEntity> items, Vector3Int position) {
        MultiValueDictionary<int, EcsEntity> map = items.toMultiValueDictionary(item => item.take<ItemFoodComponent>().foodQuality);
        for (int i = 0; i <= 5; i++) {
            if (map.ContainsKey(i)) {
                EcsEntity item = selectNearest(map[i], position);
                if (item != EcsEntity.Null) return item;
            }
        }
        return EcsEntity.Null;
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
}
}