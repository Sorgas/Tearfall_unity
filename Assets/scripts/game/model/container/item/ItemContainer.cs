using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;
using util.lang.extension;

namespace game.model.container.item {
    public class ItemContainer : EntityContainer {
        public Dictionary<Vector3Int, List<EcsEntity>> itemMap = new Dictionary<Vector3Int, List<EcsEntity>>(); // maps tiles position to list of item it that position.
        // public HashSet<EcsEntity> onMapItemsSet = new HashSet<EcsEntity>(); // set for faster checking
        // public Dictionary<EcsEntity, ItemContainerAspect> contained = new HashMap<>(); // maps contained items to containers they are in.
        public Dictionary<EcsEntity, EcsEntity> equipped = new Dictionary<EcsEntity, EcsEntity>(); // maps equipped and hauled items to units.
        public ItemFindingUtil util = new ItemFindingUtil();
        // public ContainedItemsSystem containedItemsSystem;
        public EquippedItemsManager equippedItems = new EquippedItemsManager();
        public OnMapItemsManager onMapItems = new OnMapItemsManager(); 

        private IntVector3 cachePosition;
        private LocalMap map;

        public ItemContainer() {
            // addSystem(containedItemsSystem = new ContainedItemsSystem(this));
            // addSystem(equippedItemsSystem = new EquippedItemsSystem(this));
            // addSystem(onMapItemsSystem = new OnMapItemsSystem(this));
            cachePosition = new IntVector3();
        }

        // public void addItem(EcsEntity item) {
        //     objects.add(item);
        // }
        //
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
        //
        // private LocalMap map() {
        //     return map == null ? map = GameMvc.model().get(LocalMap.class) : map;
        // }
        //
        // public boolean isItemOnMap(EcsEntity item) {
        //     return onMapItemsSet.contains(item);
        // }
        //
        // public boolean isItemInContainer(EcsEntity item) {
        //     return contained.containsKey(item);
        // }
        //
        // public boolean isItemEquipped(EcsEntity item) {
        //     return equipped.containsKey(item);
        // }

        public void registerItem(EcsEntity item) {
            // TODO handle equipped and contained items
            if (item.hasPos()) {
                onMapItems.registerItem(item);
            }
        }
    }
}