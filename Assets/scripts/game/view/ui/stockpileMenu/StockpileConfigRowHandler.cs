using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static game.view.ui.stockpileMenu.StockpileMenuLevel;
using static generation.zone.StockpileConfigItemStatus;

namespace game.view.ui.stockpileMenu {
    // handles stockpile config row button
    // shows status with button background color (enabled, disabled, mixed)
    // shows if category is shown in item types column by frame  
    public class StockpileConfigRowHandler : MonoBehaviour {
        public Image statusIndicator; // changes color basing on status
        public Button selectButton; // only shows children of this row
        public Button toggleButton; // toggles all children of this row
        public TextMeshProUGUI nameText;
        public Image selectionFrame;
        
        private StockpileConfigMenuHandler handler;
        private StockpileConfigItem configItem;
        public string category;
        public string itemType; // can be null
        public string material; // can be null

        public StockpileConfigRowHandler init(StockpileConfigMenuHandler handler, string category, string itemType, string material, StockpileConfigItem configItem) {
            this.handler = handler;
            this.category = category;
            this.itemType = itemType;
            this.material = material;
            this.configItem = configItem;
            toggleButton.onClick.AddListener(toggle);
            selectButton.onClick.AddListener(select);
            nameText.text = selectText(category, itemType, material, configItem.level);
            return this;
        }

        // changes status of this and all children rows
        private void toggle() {
            handler.toggle(category, itemType, material, configItem);
        }

        // shows children rows of this row
        private void select() {
            if (configItem.level == CATEGORY) {
                handler.selectCategory(category);
            } else if (configItem.level == ITEM_TYPE) {
                handler.selectItemType(category, itemType);
            }
        }

        // changes visual only
        public StockpileConfigRowHandler updateVisual(bool selected) {
            return configItem.status switch {
                ENABLED => setVisual(Color.green, "D", selected),
                DISABLED => setVisual(Color.red, "E", selected),
                MIXED => setVisual(Color.yellow, "M", selected),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private StockpileConfigRowHandler setVisual(Color color, string buttonText, bool selected) {
            statusIndicator.color = color;
            toggleButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
            selectionFrame.gameObject.SetActive(selected);
            return this;
        }

        private string selectText(string category, string itemType, string material, StockpileMenuLevel level) {
            return level switch {
                CATEGORY => category,
                ITEM_TYPE => itemType,
                MATERIAL => material,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }
    }
}