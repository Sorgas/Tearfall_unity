using System.Collections.Generic;
using Leopotam.Ecs;

namespace game.model.container.item {
    // stores references from items to containers they are stored in
    // only registers items
    public class StoredItemsManager {
        private Dictionary<EcsEntity, EcsEntity> storedItems = new(); // item to container

        public bool isStored(EcsEntity item) {
            return storedItems.ContainsKey(item);
        }

        public EcsEntity getContainerOfItem(EcsEntity item) {
            return storedItems[item];
        }

        public void addItemToContainer(EcsEntity item, EcsEntity container) {
            storedItems.Add(item, container);
        }

        public void removeItemFromContainer(EcsEntity item) {
            storedItems.Remove(item);
        }
    }
}