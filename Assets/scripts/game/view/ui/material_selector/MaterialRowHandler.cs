using game.view.ui.toolbar;
using TMPro;
using types.item.type;
using types.material;
using UnityEngine;
using UnityEngine.UI;
using static game.view.ui.UiColorsEnum;

namespace game.view.ui.material_selector {
    // stores item type and material. can be clicked and sets item properties to current mouse tool
    public class MaterialRowHandler : MonoBehaviour {
        public Button button;
        public Image itemImage;
        public TextMeshProUGUI text;

        private string itemType = "";
        private int material;
        private MaterialSelectionWidgetHandler widgetHandler;

        public void init(int material, string itemType, int quantity, MaterialSelectionWidgetHandler widgetHandler) {
            // TODO add material tint
            this.widgetHandler = widgetHandler;
            this.itemType = itemType;
            this.material = material;
            itemImage.sprite = ItemTypeMap.get().getSprite(itemType);
            text.text = MaterialMap.get().material(material).name + " " + itemType + ": " + quantity;
            button.onClick.AddListener(select);
        }

        public void updateSelected(string itemType, int material) {
            if (!button.interactable) return; // no changes if not enough materials
            ColorBlock colorBlock = button.colors;
            bool selected = this.itemType.Equals(itemType) && this.material == material; 
            colorBlock.normalColor = selected ? BUTTON_CHOSEN : BUTTON_NORMAL;
            button.colors = colorBlock;
        }

        public void select() {
            widgetHandler.select(itemType, material);
        }
    }
}