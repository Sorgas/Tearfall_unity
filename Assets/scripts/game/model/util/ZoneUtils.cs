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
        public static Vector3Int findFreeStockpileTile(ZoneComponent zone, StockpileComponent stockpile, LocalModel model) {
            foreach (Vector3Int tile in zone.tiles) {
                if (!model.itemContainer.onMap.itemsOnMap.ContainsKey(tile)) return tile;
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
                .Where(tile => !tracking.locked.ContainsKey(tile))
                .Count(tile =>
                    !model.itemContainer.onMap.itemsOnMap.ContainsKey(tile) || // no items on tile
                    !allItemsNotAllowedInStockpile(stockpile, model.itemContainer.onMap.itemsOnMap[tile])); // no allowed items
        }

        public static Vector3Int findUnhoedTile(ZoneComponent zone, LocalModel model) {
            foreach (Vector3Int tile in zone.tiles) {
                if (tileUnhoed(tile, model)) return tile;
            }
            return Vector3Int.back;
        }

        public static Vector3Int findNearestUnhoedTile(ZoneComponent zone, Vector3Int position, LocalModel model) {
            return zone.tiles
                .Where(tile => tileUnhoed(tile, model))
                .OrderBy(tile => PositionUtil.fastDistance(tile, position))
                .firstOrDefault(Vector3Int.back);
        }

        // returns true, if tile can be hoed
        public static bool tileUnhoed(Vector3Int tile, LocalModel model) {
            return !model.farmContainer.isFarm(tile);
        }

        // // only checks presence of desired plants. block type and material should be maintained in ZoneUpdateSystem
        // public static Vector3Int findUnplantedTile(ZoneComponent zone, FarmComponent farm, LocalModel model) {
        //     return zone.tiles
        //         .Where(tile => model.farmContainer.isFarm(tile))
        //         .Where(tile => {
        //             EcsEntity plant = model.plantContainer.getPlant(tile);
        //             if (plant == EcsEntity.Null) return true;
        //             return farm.plant == plant.take<PlantComponent>().type.name;
        //         })
        //         .First();
        // }


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
    }
}