using System.Collections.Generic;
using Leopotam.Ecs;

namespace game.model.container.item {
    // stores references from items to containers they are stored in
    // only registers items
    public class StoredItemsManager {
        private Dictionary<EcsEntity, EcsEntity> storedItems = new();

        public void addItemToContainer(EcsEntity item, EcsEntity container) {
            storedItems.Add(item, container);
        }
    }
}