using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.localmap;
using UnityEngine;
using util.lang.extension;

namespace game.model.util {
    public class ZoneUtils {
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
    }
}