using System.Collections.Generic;
using System.Linq;
using game.view.ui.util;
using game.view.util;
using generation.zone;
using Leopotam.Ecs;
using UnityEngine;
using static game.view.ui.stockpileMenu.StockpileMenuLevel;
using static generation.zone.StockpileConfigItemStatus;
using Vector3 = UnityEngine.Vector3;

namespace game.view.ui.stockpileMenu {
    public class StockpileConfigMenuHandler : MbWindow, IHotKeyAcceptor {
        private static string ROW_NAME = "StockpileCategoryRow";
        public Transform categoriesContainer;
        public Transform itemTypesContainer;
        public Transform materialsContainer;

        private EcsEntity stockpile;
        private StockpileConfiguration config = new();

        // currently displayed
        private string selectedCategory;
        private string selectedItemType;
        private readonly List<StockpileConfigRowHandler> shownCategories = new();
        private readonly List<StockpileConfigRowHandler> shownItemTypes = new();
        private readonly List<StockpileConfigRowHandler> shownMaterials = new();
        private float rowHeight;

        public void openFor(EcsEntity stockpile) {
            open();
            rowHeight = PrefabLoader.get("StockpileCategoryRow").GetComponent<RectTransform>().rect.height;
            this.stockpile = stockpile;
            config.fillStructureFromEntity(stockpile); // fills map
            createCategoryButtons();
        }

        public override void close() {
            config.saveStructureToEntity(stockpile);
            stockpile = EcsEntity.Null;
            base.close();
        }

        // enables category, item type or material
        public void toggle(string category, string itemType, string material, StockpileConfigItem configItem) {
            Debug.Log("enable " + configItem.level + " " + category + " " + itemType + " " + material + " " + configItem.status);
            StockpileConfigItemStatus status = configItem.status == ENABLED ? DISABLED : ENABLED;
            configItem.setStatus(status);
            if (configItem.level != CATEGORY) {
                config.getItemByLevel(CATEGORY, category, null, null)
                    .updateStatusByChildren(); // recursive
            }
            // update button colors
            updateButtonsState();
        }

        private void createCategoryButtons() {
            clearColumn(categoriesContainer, shownCategories);
            List<StockpileConfigItem> items = config.map.Values.ToList();
            for (var i = 0; i < items.Count; i++) {
                StockpileConfigItem item = items[i];
                if (i == 0) selectCategory(item.name);
                StockpileConfigRowHandler row = createRow(categoriesContainer, i)
                    .init(this, item.name, null, null, item);
                shownCategories.Add(row);
            }
            updateButtonsState();
        }

        // shows item types of category, show materials of first itemType
        public void selectCategory(string category) {
            selectedCategory = category;
            clearColumn(itemTypesContainer, shownItemTypes);
            List<StockpileConfigItem> items = config.map[category].children.Values.ToList();
            for (var i = 0; i < items.Count; i++) {
                StockpileConfigItem item = items[i];
                if (i == 0) selectItemType(category, item.name);
                StockpileConfigRowHandler row = createRow(itemTypesContainer, i)
                    .init(this, category, item.name, null, item);
                shownItemTypes.Add(row);
            }
            updateButtonsState();
        }

        public void selectItemType(string category, string itemType) {
            selectedCategory = category;
            selectedItemType = itemType;
            clearColumn(materialsContainer, shownMaterials);
            List<StockpileConfigItem> items = config.map[category].children[itemType].children.Values.ToList();
            for (var i = 0; i < items.Count; i++) {
                StockpileConfigItem item = items[i];
                StockpileConfigRowHandler row = createRow(materialsContainer, i)
                    .init(this, category, itemType, item.name, item);
                shownMaterials.Add(row);
            }
            updateButtonsState();
        }

        private void clearColumn(Transform container, List<StockpileConfigRowHandler> handlers) {
            foreach (Transform row in container.transform) {
                Destroy(row.gameObject);
            }
            handlers.Clear();
        }

        private StockpileConfigRowHandler createRow(Transform parent, int i) {
            GameObject row = PrefabLoader.create(ROW_NAME, parent, new Vector3(0, -rowHeight * i, 0));
            return row.GetComponent<StockpileConfigRowHandler>();
        }

        public bool accept(KeyCode key) {
            if (key == KeyCode.Q) close();
            return true;
        }

        private void updateButtonsState() {
            shownCategories.ForEach(row => row.updateVisual(row.category.Equals(selectedCategory)));
            shownItemTypes.ForEach(row => row.updateVisual(row.itemType.Equals(selectedItemType)));
            shownMaterials.ForEach(row => row.updateVisual(false));
        }
    }

    public enum StockpileMenuLevel {
        CATEGORY,
        ITEM_TYPE,
        MATERIAL
    }
}