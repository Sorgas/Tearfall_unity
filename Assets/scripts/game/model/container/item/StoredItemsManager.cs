using game.model.component;
using game.model.component.item;
using game.model.localmap;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.container.item {
    // stores references from items to containers they are stored in
    // should be the only place where items transferred in or from container.
    // should be the only place where ItemContainedComponent and ItemContainerComponent are modified.
    public class StoredItemsManager : ItemContainerPart {
        // commented, because map duplicates capability of ItemContainedComponent
        // private Dictionary<EcsEntity, EcsEntity> storedItems = new(); // item to container

        public StoredItemsManager(LocalModel model, ItemContainer container) : base(model, container) { }
        
        public bool isStored(EcsEntity item) {
            return item.Has<ItemContainedComponent>();
            // return storedItems.ContainsKey(item);
        }
        
        public void addItemToContainer(EcsEntity item, EcsEntity containerEntity) {
            container.availableItemsManager.add(item);
            containerEntity.take<ItemContainerComponent>().items.Add(item);
            item.Replace(new ItemContainedComponent { container = containerEntity });
            // storedItems.Add(item, containerEntity);
        }

        public void removeItemFromContainer(EcsEntity item) {
            container.availableItemsManager.remove(item);
            item.take<ItemContainedComponent>().container.take<ItemContainerComponent>().items.Remove(item);
            item.Del<ItemContainedComponent>();
            // storedItems[item].take<ItemContainerComponent>().items.Remove(item);
            // storedItems.Remove(item);
        }
    }
}