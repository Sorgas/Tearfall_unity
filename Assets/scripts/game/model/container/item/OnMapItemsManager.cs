using System.Collections.Generic;
using game.model.component;
using game.model.component.item;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.container.item {
    // stores reference of positions to items in it. Puts and takes items from map, updating its entity's position component 
    public class OnMapItemsManager {
        public Dictionary<Vector3Int, List<EcsEntity>> itemsOnMap = new Dictionary<Vector3Int, List<EcsEntity>>();
        public HashSet<EcsEntity> all = new HashSet<EcsEntity>();

        // adds item to map by its position (used on generation and loading)
        public void addItemToMap(EcsEntity item) {
            Vector3Int position = item.pos();
            if (!itemsOnMap.ContainsKey(position)) itemsOnMap.Add(position, new List<EcsEntity>());
            itemsOnMap[position].Add(item);
            all.Add(item);
        }

        // adds item without position to specified position on map
        public void putItemToMap(EcsEntity item, Vector3Int position) {
            validateForPlacing(item);
            item.Replace(new PositionComponent { position = position });
            addItemToMap(item);
        }

        // removes item from map
        public void takeItemFromMap(EcsEntity item) {
            validateForTaking(item);
            Vector3Int position = item.pos();
            item.Del<PositionComponent>();
            itemsOnMap[position].Remove(item);
            all.Remove(item);
            if (itemsOnMap[position].Count == 0) {
                itemsOnMap.Remove(position); // remove empty list
            }
        }

        // validate that item is registered on its position
        private void validateForTaking(EcsEntity item) {
            if (!item.hasPos()) {
                Debug.LogError("Item " + item + " has no position.");
            }
            Vector3Int position = item.pos();
            if (!itemsOnMap.ContainsKey(position)) {
                Debug.LogError("Tile on " + item + " position is empty.");
            }
            if (!itemsOnMap[position].Contains(item)) {
                Debug.LogError("Item " + item + " is not registered by its position.");
            }
        }

        // validates that item has no position 
        private void validateForPlacing(EcsEntity item) {
            if (item.hasPos()) {
                Debug.LogError("Item " + item + " already has position.");
            }
        }
    }
}