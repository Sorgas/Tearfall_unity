using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.component.plant;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.material;
using UnityEngine;
using util.geometry;
using util.lang.extension;

namespace game.model.util {
    public class ZoneUtils {
        private const string SOIL_TAG = "soil";

        public static Vector3Int findFreeStockpileCells(ZoneComponent zone, StockpileComponent stockpile, LocalModel model) {
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

        public static int countFreeStockpileCells(ZoneComponent zone, StockpileComponent stockpile, LocalModel model) {
            return zone.tiles
                .Count(tile => !model.itemContainer.onMap.itemsOnMap.ContainsKey(tile) ||
                               !model.itemContainer.onMap.itemsOnMap[tile]
                                   .Select(item => item.take<ItemComponent>())
                                   .Any(item => stockpile.map.ContainsKey(item.type) && stockpile.map[item.type].Contains(item.material)));
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

        public static Vector3Int findUnplantedTile(ZoneComponent zone, FarmComponent farm, LocalModel model) {
            return zone.tiles
                .Where(tile => model.localMap.blockType.get(tile) == BlockTypes.FARM.CODE)
                .Where(tile => {
                    EcsEntity plant = model.plantContainer.getPlant(tile);
                    if (plant == EcsEntity.Null) return false;
                    return farm.config.Contains(plant.take<PlantComponent>().type.name);
                })
                .First();
        }
    }
}