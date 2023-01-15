using game.view.system.mouse_tool;
using game.view.ui.toolbar;
using TMPro;
using types.item.type;
using types.material;
using UnityEngine;
using UnityEngine.UI;

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
            ColorBlock colorBlock = button.colors;
            bool selected = 
                // this.itemType.Equals(itemType) && 
                this.material == material; 
            colorBlock.normalColor = selected ? Color.yellow : Color.white;
            button.colors = colorBlock;
            button.interactable = !selected;
        }

        public void select() {
            MouseToolManager.get().setItem(itemType, material);
            widgetHandler.updateSelected(itemType, material);
        }
    }
}