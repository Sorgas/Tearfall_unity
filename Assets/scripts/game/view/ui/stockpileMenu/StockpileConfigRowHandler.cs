using System;
using game.model.component;
using generation.zone;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static generation.zone.StockpileConfigItemStatus;

namespace game.view.ui.stockpileMenu {
    // handles stockpile config row button
    // shows status with button background color (enabled, disabled, mixed)
    // shows if category is shown in item types column by frame  
    public class StockpileConfigRowHandler : MonoBehaviour {
        public Image statusIndicator;
        public Button selectButton;
        public Button toggleButton;
        public TextMeshProUGUI nameText;

        private StockpileConfigMenuHandler handler;
        private EcsEntity stockpile;
        private string category;
        private string itemType;
        private string material;
        private StockpileMenuLevel level;
        public StockpileConfigItemStatus status;

        public StockpileConfigRowHandler init(StockpileConfigMenuHandler handler, EcsEntity stockpile, string category, string itemType, string material, StockpileMenuLevel level) {
            this.handler = handler;
            this.stockpile = stockpile;
            this.category = category;
            this.itemType = itemType;
            this.material = material;
            this.level = level;
            toggleButton.onClick.AddListener(() => toggle());

            nameText.text = category; //TODO switch
            return this;
        }

        public void toggle() {
            handler.enable(level, category, itemType, material, status == ENABLED ? DISABLED : ENABLED);
        }

        public void select() {
            switch (level) {
                case StockpileMenuLevel.CATEGORY:
                    handler.selectCategory(category);
                    break;
                case StockpileMenuLevel.ITEM_TYPE:
                    handler.selectItemType(category, itemType);
                    break;
                case StockpileMenuLevel.MATERIAL:
                    // unselectable
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // changes visual only
        public StockpileConfigRowHandler setStatus(StockpileConfigItemStatus status) {
            switch (status) {
                case ENABLED:
                    statusIndicator.color = Color.green;
                    toggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "D";
                    break;
                case DISABLED:
                    statusIndicator.color = Color.red;
                    toggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "E";
                    break;
                case MIXED:
                    statusIndicator.color = Color.yellow;
                    toggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "M";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return this;
        }
    }
}