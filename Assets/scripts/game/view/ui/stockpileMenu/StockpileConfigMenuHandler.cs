using System.Collections.Generic;
using game.view.ui.util;
using game.view.util;
using generation.zone;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;
using util.lang.extension;
using static game.view.ui.stockpileMenu.StockpileMenuLevel;
using static generation.zone.StockpileConfigItemStatus;
using StockpileComponent = game.model.component.StockpileComponent;
using StockpileConfigItem = generation.zone.StockpileConfigItem;
using Vector3 = UnityEngine.Vector3;

namespace game.view.ui.stockpileMenu {
    public class StockpileConfigMenuHandler : MbWindow {
        private static string ROW_NAME = "StockpileCategoryRow";

        public Transform categoriesContainer;
        public Transform itemTypesContainer;
        public Transform materialsContainer;

        public EcsEntity stockpile;
        private Dictionary<string, StockpileConfigItem> map = new();

        // currently displayed
        private string selectedCategory;
        private string selectedItemType;
        private Dictionary<string, StockpileConfigRowHandler> shownCategories = new();
        private Dictionary<string, StockpileConfigRowHandler> shownItemTypes = new();
        
        private StockpileInitializer initializer = new();
        private float rowHeight;

        public void init() {
            initializer.init();
            rowHeight = PrefabLoader.get("StockpileCategoryRow").GetComponent<RectTransform>().rect.height;
        }

        public void initFor(EcsEntity stockpile) {
            this.stockpile = stockpile;
            createConfigItemStructure();
            createCategoryButtons();
        }

        // enables category, item type or material.
        public void enable(StockpileMenuLevel level, string categoryName, string itemTypeName, string materialName, StockpileConfigItemStatus status) {
            StockpileConfigItem itemToSet = getItemByLevel(level, categoryName, itemTypeName, materialName);
            setEnableStatusToItem(itemToSet, status);
            if (level != CATEGORY) updateStatusByChildren(map[categoryName]);
        }

        private StockpileConfigItem getItemByLevel(StockpileMenuLevel level, string categoryName, string itemTypeName, string materialName) {
            switch (level) {
                case CATEGORY:
                    return map[categoryName];
                case ITEM_TYPE:
                    return map[categoryName].children[itemTypeName];
                case MATERIAL:
                    return map[categoryName].children[itemTypeName].children[materialName];
            }
            return null; // never reached
        }

        // recursively sets new status to item and its children
        private void setEnableStatusToItem(StockpileConfigItem item, StockpileConfigItemStatus newStatus) {
            item.status = newStatus;
            if (item.children == null) return;
            item.children.Values.ForEach(child => setEnableStatusToItem(child, newStatus));
        }

        // recursively update statuses from children to parents
        private void updateStatusByChildren(StockpileConfigItem item) {
            item.children.Values.ForEach(child => updateStatusByChildren(child));
            item.status = getStatusByChildren(item);
            if (selectedCategory == item.name) {
                shownCategories[selectedCategory].status = item.status;
            }
            if (selectedItemType == item.name) {
                shownItemTypes[selectedItemType].status = item.status;
            }
        }
        
        private void createCategoryButtons() {
            clearContainer(categoriesContainer);
            shownCategories.Clear();
            int i = 0;
            foreach (StockpileConfigItem item in map.Values) {
                StockpileConfigRowHandler row = createRow(categoriesContainer, i++)
                    .init(this, stockpile, selectedCategory, item.name, null, ITEM_TYPE)
                    .setStatus(item.status);
                shownCategories.Add(item.name, row);
            }
        }

        // shows item types of category, show materials of first itemType
        public void selectCategory(string category) {
            clearContainer(itemTypesContainer);
            shownItemTypes.Clear();
            int i = 0;
            foreach (StockpileConfigItem item in map[category].children.Values) {
                StockpileConfigRowHandler row = createRow(itemTypesContainer, i++)
                    .init(this, stockpile, category, item.name, null, ITEM_TYPE)
                    .setStatus(item.status);
                shownItemTypes.Add(item.name, row);
            }
            selectedCategory = category;
        }

        public void selectItemType(string category, string itemType) {
            StockpileConfigItem itemTypeItem = map[category].children[itemType];
            clearContainer(materialsContainer);
            int i = 0;
            foreach (StockpileConfigItem item in itemTypeItem.children.Values) {
                createRow(materialsContainer, i++)
                    .init(this, stockpile, category, itemType, item.name, MATERIAL)
                    .setStatus(item.status);
            }
            selectedCategory = category;
            selectedItemType = itemType;
        }

        // creates 3-level tree structure for current stockpile for easier configuring
        private void createConfigItemStructure() {
            map.Clear();
            foreach (KeyValuePair<string, StockpileConfigItem> pair in initializer.prototype) {
                map.Add(pair.Key, pair.Value.clone());
            }
            MultiValueDictionary<string, string> stockpileMap = stockpile.take<StockpileComponent>().map;
            foreach (StockpileConfigItem category in map.Values) {
                foreach (KeyValuePair<string, StockpileConfigItem> itemTypePair in category.children) {
                    foreach (KeyValuePair<string, StockpileConfigItem> materialPair in itemTypePair.Value.children) {
                        materialPair.Value.status = stockpileMap.contains(itemTypePair.Key, materialPair.Key) ? ENABLED : DISABLED;
                    }
                    StockpileConfigItemStatus status = DISABLED;
                    if (stockpileMap.ContainsKey(itemTypePair.Key)) {
                        if (stockpileMap[itemTypePair.Key].Count == category.children.Count) {
                            status = ENABLED; // all materials for item type
                        } else {
                            status = MIXED; // not all materials for item type
                        }
                    }
                    itemTypePair.Value.status = status;
                }
                category.status = getStatusByChildren(category);
            }
        }

        // combines statuses of all children. All enabled -> enabled, all disabled -> disabled, mixed otherwise.
        private StockpileConfigItemStatus getStatusByChildren(StockpileConfigItem item) {
            if (item.children == null) return item.status;
            bool hasEnabled = false;
            bool hasDisabled = false;
            foreach (StockpileConfigItem child in item.children.Values) {
                switch (child.status) {
                    case MIXED:
                        return MIXED;
                    case ENABLED:
                        hasEnabled = true;
                        break;
                    case DISABLED:
                        hasDisabled = true;
                        break;
                }
                if (hasEnabled && hasDisabled) return MIXED;
            }
            return hasEnabled ? ENABLED : DISABLED;
        }

        private void clearContainer(Transform container) {
            foreach (Transform row in container.transform) {
                Destroy(row.gameObject);
            }
        }

        private StockpileConfigRowHandler createRow(Transform parent, int i) {
            GameObject row = PrefabLoader.create(ROW_NAME, parent, new Vector3(0, -rowHeight * i, 0));
            return row.GetComponent<StockpileConfigRowHandler>();
        }
    }

    public enum StockpileMenuLevel {
        CATEGORY,
        ITEM_TYPE,
        MATERIAL
    }
}