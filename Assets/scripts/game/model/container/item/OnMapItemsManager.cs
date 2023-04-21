using System.Collections.Generic;
using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;
using util.lang.extension;

namespace game.model.container.item {
    // stores reference of positions to items in it. Puts and takes items from map, updating its entity's position component 
    public class OnMapItemsManager : ItemContainerPart {
        public readonly MultiValueDictionary<Vector3Int, EcsEntity> itemsOnMap = new(); // to find what items are on position
        public readonly HashSet<EcsEntity> all = new(); // for checking if item is on map

        public OnMapItemsManager(LocalModel model, ItemContainer container) : base(model, container) { }
        
        public List<EcsEntity> getItems(Vector3Int position) {
            if (itemsOnMap.ContainsKey(position)) {
                return itemsOnMap[position];
            }
            return new List<EcsEntity>();
        }

        // adds item without position to specified position on map (used in gameplay)
        public void putItemToMap(EcsEntity item, Vector3Int position) {
            container.validator.validateForPlacing(item);
            item.Replace(new PositionComponent { position = position });
            addItemToMap(item);
        }

        // removes item from map
        public void takeItemFromMap(EcsEntity item) {
            container.validator.validateForTaking(item);
            Vector3Int position = item.pos();
            item.Del<PositionComponent>();
            itemsOnMap.remove(position, item);
            all.Remove(item);
            container.availableItemsManager.remove(item); // make item unavailable
            addPositionForUpdate(position);
        }

        // adds item to map by its position (used on generation and loading)
        private void addItemToMap(EcsEntity item) {
            itemsOnMap.add(item.pos(), item);
            all.Add(item);
            container.availableItemsManager.add(item); // make item available
            addPositionForUpdate(item.pos());
        }
    }
}