using System;
using System.Collections.Generic;
using game.model.component;
using generation.zone;
using Leopotam.Ecs;
using util.lang;
using util.lang.extension;
using static game.view.ui.stockpileMenu.StockpileMenuLevel;
using static generation.zone.StockpileConfigItemStatus;

namespace game.view.ui.stockpileMenu {
    // stores tree of config items
    public class StockpileConfiguration {
        public readonly StockpileInitializer initializer = new();
        public Dictionary<string, StockpileConfigItem> map = new();

        // creates 3-level tree structure for current stockpile for easier configuring
        public void fillStructureFromEntity(EcsEntity stockpile) {
            refillMap();
            readItemsStatuses(stockpile);
        }

        public void saveStructureToEntity(EcsEntity stockpile) {
            StockpileComponent component = stockpile.take<StockpileComponent>();
            component.map.Clear();
            foreach (StockpileConfigItem category in map.Values) {
                foreach (StockpileConfigItem itemType in category.children.Values) {
                    foreach (StockpileConfigItem material in itemType.children.Values) {
                        if(material.status == ENABLED) component.map.add(itemType.name, material.id);
                    }
                }
            }
        }
        
        public StockpileConfigItem getItemByLevel(StockpileMenuLevel level, string categoryName, string itemTypeName, string materialName) {
            return level switch {
                CATEGORY => map[categoryName],
                ITEM_TYPE => map[categoryName].children[itemTypeName],
                MATERIAL => map[categoryName].children[itemTypeName].children[materialName],
                _ => throw new ArgumentException() // never reached
            };
        }

        private void refillMap() {
            map.Clear();
            if (!initializer.loaded) initializer.init();
            foreach (KeyValuePair<string, StockpileConfigItem> pair in initializer.prototype) {
                map.Add(pair.Key, pair.Value.clone());
            }
        }

        private void readItemsStatuses(EcsEntity stockpile) {
            MultiValueDictionary<string, int> stockpileMap = stockpile.take<StockpileComponent>().map;
            foreach (StockpileConfigItem category in map.Values) {
                foreach (StockpileConfigItem itemType in category.children.Values) {
                    foreach (StockpileConfigItem material in itemType.children.Values) {
                        material.status = stockpileMap.contains(itemType.name, material.id) ? ENABLED : DISABLED;
                    }
                    StockpileConfigItemStatus status = DISABLED;
                    if (stockpileMap.ContainsKey(itemType.name)) {
                        if (stockpileMap[itemType.name].Count == itemType.children.Count) {
                            status = ENABLED; // all materials for item type
                        } else {
                            status = MIXED; // not all materials for item type
                        }
                    }
                    itemType.status = status;
                }
                category.status = category.getStatusByChildren();
            }
        }
    }
}