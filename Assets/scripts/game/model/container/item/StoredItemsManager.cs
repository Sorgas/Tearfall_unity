using System.Collections.Generic;
using game.model.component;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.container.item {
    // stores references from items to containers they are stored in
    // should be the only place where items transferred in or from container.
    public class StoredItemsManager {
        private Dictionary<EcsEntity, EcsEntity> storedItems = new(); // item to container

        public bool isStored(EcsEntity item) {
            return storedItems.ContainsKey(item);
        }

        public EcsEntity getContainerOfItem(EcsEntity item) {
            return storedItems[item];
        }

        public void addItemToContainer(EcsEntity item, EcsEntity container) {
            container.take<ItemContainerComponent>().items.Add(item);
            storedItems.Add(item, container);
        }

        public void removeItemFromContainer(EcsEntity item) {
            storedItems[item].take<ItemContainerComponent>().items.Remove(item);
            storedItems.Remove(item);
        }
    }
}