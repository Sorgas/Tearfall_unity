namespace game.model.container.item {
    // stores all items on the level. has separate storage classes for items on ground, stored in containers, equipped on units.
    // transitions are made in actions.
    public class  ItemContainer : EntityContainer {
        public ItemStateValidator validator;
        public EquippedItemsManager equippedItems = new();
        public OnMapItemsManager onMapItems;
        // TODO stored items
        // TODO player owned items
        
        public AvailableItemsManager availableItemsManager = new();
        public ItemFindingUtil util = new();

        public ItemContainer() {
            validator = new(this);
            onMapItems = new(this);
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
        // public boolean itemAccessible(EcsEntity item, IntVector3 position) {
        //     //TODO handle items in containers
        //     if (isItemInContainer(item)) {
        //         IntVector3 containerPosition = contained.get(item).entity.position;
        //         LocalMap map = map();
        //         byte area = map.passageMap.area.get(position);
        //         return PositionUtil.allNeighbour.stream()
        //             .map(pos->IntVector3.add(pos, containerPosition))
        //             .filter(map::inMap)
        //             .map(map.passageMap.area::get)
        //             .anyMatch(area1->area1 == area);
        //     }
        //     return item.position != null && map().passageMap.area.get(position) == map().passageMap.area.get(item.position);
        // }
    }
}