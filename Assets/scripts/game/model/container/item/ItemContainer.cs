using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.container.item {
    // stores all items on the level. has separate storage classes for items on ground, stored in containers, equipped on units.
    // transitions are made in actions. Does not consider items locking.
    public class ItemContainer : LocalMapModelComponent {
        public ItemStateValidator validator;
        public EquippedItemsManager equipped = new();
        public OnMapItemsManager onMap;
        public StoredItemsManager stored = new();
        
        public AvailableItemsManager availableItemsManager = new();
        public ItemFindingUtil util;
        public ItemTransitionUtil transition;
        
        public ItemContainer(LocalModel model) : base(model) {
            validator = new(model, this);
            onMap = new(model, this);
            util = new(model, this);
            transition = new(model, this);
        }

        // public void removeItem(EcsEntity item) {
        //     if (!objects.contains(item)) {
        //         Logger.ITEMS.logWarn("Removing not present item " + item.type.name);
        //     } else {
        //         Logger.ITEMS.logDebug("Removing item " + item.type.name);
        //     }
        //     if (isItemOnMap(item)) onMapItemsSystem.removeItemFromMap(item);
        //     if (isItemInContainer(item)) containedItemsSystem.removeItemFromContainer(item);
        //     if (isItemEquipped(item)) equippedItemsSystem.removeItemFromEquipment(item);
        //     item.destroyed = true;
        //     objects.remove(item);
        // }
        //
        // public void removeItems(List<EcsEntity> items) {
        //     items.forEach(this::removeItem);
        // }
        //
        // public List<EcsEntity> getItemsInPosition(IntVector3 position) {
        //     return new ArrayList<>(itemMap.getOrDefault(position, Collections.emptyList()));
        // }
        //
        // public List<EcsEntity> getItemsInPosition(int x, int y, int z) {
        //     return getItemsInPosition(cachePosition.set(x, y, z));
        // }
        //

        // checks that items's position is accessible from 'position'
        public bool itemAccessibleFromPosition(EcsEntity item, Vector3Int position) {
            Vector3Int targetPosition = (stored.isStored(item) ? stored.getContainerOfItem(item) : item).pos();
            return model.localMap.passageMap.tileIsAccessibleFromArea(targetPosition, position);               
        }
    }

    public class ItemContainerPart : LocalMapModelComponent {
        protected ItemContainer container;

        public ItemContainerPart(LocalModel model, ItemContainer container) : base(model) {
            this.container = container;
        }
    }
}