using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;
using util.lang.extension;

namespace game.model.util {
    // TODO check locking
    public class ZoneUtils {
        private const string SOIL_TAG = "soil";

        // TODO add task parameter and check locking by other tasks
        public static Vector3Int findFreeStockpileTile(EcsEntity zone, EcsEntity task, LocalModel model) {
            ZoneComponent zoneComponent = zone.take<ZoneComponent>();
            StockpileComponent stockpile = zone.take<StockpileComponent>();
            foreach (Vector3Int tile in zoneComponent.tiles) {
                if (!tileCanBeLocked(zone, tile, task)) continue; // tile locked to another task
                if (!model.itemContainer.onMap.itemsOnMap.ContainsKey(tile)) return tile; // no items on tile
                bool hasStoredItem = model.itemContainer.onMap.itemsOnMap[tile]
                    .Select(item => item.take<ItemComponent>())
                    .Any(item => stockpile.map.ContainsKey(item.type) && stockpile.map[item.type].Contains(item.material));
                if (!hasStoredItem) {
                    return tile;
                }
            }
            return Vector3Int.back;
        }

        // counts tile that can accept brought item
        public static int countFreeStockpileTiles(ZoneComponent zone, StockpileComponent stockpile, ZoneTrackingComponent tracking, LocalModel model) {
            return zone.tiles
                .Where(tile => !tracking.locked.ContainsKey(tile)) // tile not locked by some existing task
                .Count(tile =>
                    !model.itemContainer.onMap.itemsOnMap.ContainsKey(tile) || // no items on tile
                    !allItemsNotAllowedInStockpile(stockpile, model.itemContainer.onMap.itemsOnMap[tile])); // no allowed items
        }

        // tile not locked, not farm
        public static Vector3Int findUnhoedTile(EcsEntity entity, LocalModel model) {
            return findUnhoedTiles(entity, model).firstOrDefault(Vector3Int.back);
        }

        // tile not locked, not farm
        public static Vector3Int findNearestUnhoedTile(EcsEntity entity, Vector3Int position, LocalModel model) {
            return findUnhoedTiles(entity, model)
                .OrderBy(tile => PositionUtil.fastDistance(tile, position))
                .firstOrDefault(Vector3Int.back);
        }

        private static IEnumerable<Vector3Int> findUnhoedTiles(EcsEntity entity, LocalModel model) {
            var zone = entity.take<ZoneComponent>();
            var tracking = entity.take<ZoneTrackingComponent>();
            return zone.tiles
                .Where(tile => !tracking.locked.ContainsKey(tile)) // tile not locked by some existing task
                .Where(tile => !tileHoed(tile, model));
        }

        public static int countUnhoedTiles(ZoneComponent zone, ZoneTrackingComponent tracking, LocalModel model) {
            return zone.tiles
                .Where(tile => !tracking.locked.ContainsKey(tile)) // tile not locked by some existing task
                .Count(tile => !tileHoed(tile, model));
        }

        // returns true, if tile can be hoed
        public static bool tileHoed(Vector3Int tile, LocalModel model) => model.farmContainer.isFarm(tile);

        // only checks presence of desired plants. block type and material should be maintained in ZoneUpdateSystem
        public static Vector3Int findUnplantedTile(EcsEntity entity, LocalModel model) {
            return findUnplantedTiles(entity, model).First();
        }

        // only checks presence of desired plants. block type and material should be maintained in ZoneUpdateSystem
        public static Vector3Int findNearestUnplantedTile(EcsEntity entity, Vector3Int position, LocalModel model) {
            return findUnplantedTiles(entity, model)
                .OrderBy(tile => PositionUtil.fastDistance(tile, position))
                .firstOrDefault(Vector3Int.back);
        }

        private static IEnumerable<Vector3Int> findUnplantedTiles(EcsEntity entity, LocalModel model) {
            ZoneComponent zone = entity.take<ZoneComponent>();
            ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
            FarmComponent farm = entity.take<FarmComponent>();
            return zone.tiles
                .Where(tile => !tracking.locked.ContainsKey(tile)) // tile not locked by some existing task
                .Where(tile => model.farmContainer.isFarm(tile))
                .Where(tile => model.plantContainer.getPlant(tile) == EcsEntity.Null);
        }

        public static bool itemAllowedInStockpile(StockpileComponent stockpile, ItemComponent item) {
            return stockpile.map.ContainsKey(item.type) && stockpile.map[item.type].Contains(item.material);
        }

        public static bool allItemsAllowedInStockpile(StockpileComponent stockpile, List<EcsEntity> items) {
            return items.Select(item => item.take<ItemComponent>())
                .All(item => itemAllowedInStockpile(stockpile, item));
        }

        public static bool allItemsNotAllowedInStockpile(StockpileComponent stockpile, List<EcsEntity> items) {
            return items.Select(item => item.take<ItemComponent>())
                .All(item => !itemAllowedInStockpile(stockpile, item));
        }

        public static bool tileCanBeLocked(EcsEntity zone, Vector3Int tile, EcsEntity task) {
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            return !tracking.locked.ContainsKey(tile) || tracking.locked[tile] == task;
        }
        
        public static void lockZoneTile(EcsEntity zone, Vector3Int tile, EcsEntity task) {
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            if (!ZoneUtils.tileCanBeLocked(zone, tile, task)) {
                if (tracking.locked[tile] != task) throw new ArgumentException("Cannot lock tile. Tile locked to another task");
                // already locked to this task
            } else {
                if (!tracking.locked.ContainsKey(tile)) tracking.locked.Add(tile, task);
            }
        }

        public static void unlockZoneTile(EcsEntity zone, Vector3Int tile, EcsEntity task) {
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            if (!tracking.locked.ContainsKey(tile)) return; // already unlocked
            if (tracking.locked[tile] != task) throw new ArgumentException("Cannot unlock tile. Tile locked to another task");
            tracking.locked.Remove(tile);
        }
    }
}