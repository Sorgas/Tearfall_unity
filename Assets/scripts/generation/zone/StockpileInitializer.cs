using System.Collections.Generic;
using System.Linq;
using game.model.component;
using Leopotam.Ecs;
using types.item.type;
using types.material;
using util.lang;
using util.lang.extension;

namespace generation.zone {
    public class StockpileInitializer {
        public readonly Dictionary<string, StockpileConfigItem> prototype = new();

        public void init() {
            MultiValueDictionary<string, ItemType> map = ItemTypeMap.getAll().Aggregate(new MultiValueDictionary<string, ItemType>(), (map, type) => {
                map.add(type.stockpileCategory, type);
                return map;
            });
            foreach (var pair in map) {
                StockpileConfigItem category = new(pair.Key);
                foreach (ItemType itemType in pair.Value) {
                    StockpileConfigItem itemTypeConfigItem = new(itemType.name);
                    foreach (Material_ material in MaterialMap.get().getByTagsAny(itemType.stockpileMaterialTags)) {
                        itemTypeConfigItem.children.Add(material.name, new(material.name));
                    }
                    category.children.Add(itemType.name, itemTypeConfigItem);
                }
                prototype.Add(category.name, category);
            }
        }

        public void initEntity(EcsEntity entity) {
            StockpileComponent component = entity.take<StockpileComponent>();
            foreach (StockpileConfigItem category in prototype.Values) {
                foreach (StockpileConfigItem itemTypeItem in category.children.Values) {
                    foreach (StockpileConfigItem materialItem in itemTypeItem.children.Values) {
                        component.map.add(itemTypeItem.name, materialItem.name);
                    }
                }
            }
        }
    }

    public class StockpileConfigItem {
        public string name;
        public Dictionary<string, StockpileConfigItem> children;
        public StockpileConfigItemStatus status;
        
        public StockpileConfigItem(string name) {
            this.name = name;
            children = new();
        }

        public StockpileConfigItem clone() {
            StockpileConfigItem clone = new StockpileConfigItem(name);
            foreach (KeyValuePair<string,StockpileConfigItem> pair in children) {
                clone.children.Add(pair.Key, pair.Value.clone());
            }
            return clone;
        }
    }
    
    public enum StockpileConfigItemStatus {
        ENABLED,
        DISABLED,
        MIXED
    }
}