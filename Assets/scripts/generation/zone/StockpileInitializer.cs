using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.view.ui.stockpileMenu;
using Leopotam.Ecs;
using types.item.type;
using types.material;
using util.lang;
using util.lang.extension;
using static game.view.ui.stockpileMenu.StockpileMenuLevel;

namespace generation.zone {
    // stores prototype tree-structure of stockpile categories->itemTypes->materials
    public class StockpileInitializer {
        public readonly Dictionary<string, StockpileConfigItem> prototype = new();
        public bool loaded;
        
        public void init() {
            // aggregate by stockpile category
            MultiValueDictionary<string, ItemType> categories = ItemTypeMap.getAll()
                .Aggregate(new MultiValueDictionary<string, ItemType>(), (map, type) => {
                    map.add(type.stockpileCategory ?? "null", type);
                    return map;
                });
            foreach (var pair in categories) {
                StockpileConfigItem category = new(pair.Key, CATEGORY);
                foreach (ItemType itemType in pair.Value) {
                    StockpileConfigItem itemTypeConfigItem = new(itemType.name, ITEM_TYPE);
                    MaterialMap.get()
                        .getByTagsAny(itemType.stockpileMaterialTags)
                        .Where(material => !material.isVariant)
                        .Select(material => new StockpileConfigItem(material.name, MATERIAL, material.id))
                        .ForEach(configItem => itemTypeConfigItem.children.Add(configItem.name, configItem));
                    category.children.Add(itemType.name, itemTypeConfigItem);
                }
                prototype.Add(category.name, category);
            }
            loaded = true;
        }

        public void initEntity(EcsEntity entity) {
            StockpileComponent component = entity.take<StockpileComponent>();
            foreach (StockpileConfigItem category in prototype.Values) {
                foreach (StockpileConfigItem itemTypeItem in category.children.Values) {
                    foreach (StockpileConfigItem materialItem in itemTypeItem.children.Values) {
                        component.map.add(itemTypeItem.name, materialItem.id);
                    }
                }
            }
        }
    }
    
    public enum StockpileConfigItemStatus {
        ENABLED,
        DISABLED,
        MIXED
    }
}