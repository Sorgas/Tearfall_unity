using System.Collections.Generic;
using System.Linq;
using game.view.ui.toolbar;
using game.view.util;
using Leopotam.Ecs;
using MoreLinq;
using TMPro;
using types.building;
using types.item.type;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using util.lang;
using util.lang.extension;

namespace game.view.ui.material_selector {
    // handles button of item type. controls tooltip with materials.
    // will be disabled, if there are not enough items found
    public class MaterialButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public MaterialTooltipHandler tooltip;
        public Image itemImage;
        public TextMeshProUGUI quantityText;
        public Image buttonImage;
        public Button button;

        private string itemType;
        private Dictionary<int, MaterialRowHandler> rows = new(); // material -> row
        private bool mouseInButton;
        public bool mouseInTooltip;

        public void init(BuildingVariant variant, MultiValueDictionary<int, EcsEntity> items, MaterialSelectionWidgetHandler widgetHandler) {
            itemType = variant.itemType;
            itemImage.sprite = ItemTypeMap.get().getSprite(variant.itemType);
            tooltip.close();
            fillRows(variant, items, widgetHandler);
            if (!items.Values.Any(list => list.Count >= variant.amount)) {
                button.interactable = false;
            }
            button.onClick.AddListener(() => tooltip.open());
        }

        public void mouseExitedTooltip() {
            mouseInTooltip = false;
            checkTooltipClosing();
        }

        public void OnPointerEnter(PointerEventData eventData) => mouseInButton = true;

        public void OnPointerExit(PointerEventData eventData) {
            mouseInButton = false;
            checkTooltipClosing();
        }

        public bool hasEnoughItems() {
            return button.enabled;
        }

        public void updateSelected(string itemType, int material) {
            if (!button.interactable) return;
            ColorBlock colorBlock = button.colors;
            bool selected = this.itemType == itemType;
            colorBlock.normalColor = selected ? UiColorsEnum.BUTTON_CHOSEN : UiColorsEnum.BUTTON_NORMAL;
            button.colors = colorBlock;
            rows.Values.ForEach(row => row.updateSelected(itemType, material));
        }

        public void selectAny() {
            List<MaterialRowHandler> enabledRows = rows.Values.Where(row => row.button.interactable).ToList();
            if (enabledRows.Count > 0) {
                enabledRows[0].select();
            } else {
                Debug.LogError("trying to select any material for disabled item type");
            }
        }

        // creates row for each material of items. 
        private void fillRows(BuildingVariant variant, MultiValueDictionary<int, EcsEntity> items, MaterialSelectionWidgetHandler widgetHandler) {
            RectTransform tooltipTransform = tooltip.GetComponent<RectTransform>();
            tooltipTransform.sizeDelta = new Vector2(tooltipTransform.sizeDelta.x, 50 * items.Keys.Count);
            int i = 0;
            foreach (int materialId in items.Keys) {
                GameObject row = PrefabLoader.create("materialRow", tooltip.transform, new Vector3(0, i * 50, 0));
                MaterialRowHandler rowHandler = row.GetComponent<MaterialRowHandler>();
                rowHandler.init(materialId, variant.itemType, items[materialId].Count, widgetHandler);
                rows.Add(materialId, rowHandler);
                if (items[materialId].Count < variant.amount) {
                    rowHandler.button.interactable= false;
                }
                i++;
            }
        }

        private void checkTooltipClosing() {
            if (!mouseInButton && !mouseInTooltip) {
                tooltip.close();
            }
        }
    }
}