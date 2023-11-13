using TMPro;
using types.item.type;
using types.material;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.workbench {
// combined handler for type and materials rows in crafting order ingredient configure panel.
public class CraftingOrderConfigLine : MonoBehaviour {
    public Image background;
    public Image icon;
    public TextMeshProUGUI text;
    public TextMeshProUGUI quantityText;
    public string type;
    public int material;

    public void initType(string type, int quantity, bool selected) {
        text.text = type;
        quantityText.text = quantity.ToString();
        setSelected(selected);
        setIcon(type, Color.white);
    }
    
    public void initMaterial(string type, int materialId, int quantity, bool selected) {
        Material_ material = MaterialMap.get().material(materialId);
        this.material = materialId;
        text.text = material.name + " " + type;
        quantityText.text = quantity.ToString();
        setSelected(selected);
        setIcon(type, material.color);
    }

    public void setSelected(bool value) {
        background.color = value ? UiColorsEnum.BACKGROUND_HIGHLIGHT : UiColorsEnum.BACKGROUND;
    }

    private void setIcon(string itemType, Color color) {
        icon.sprite = ItemTypeMap.get().getSprite(itemType);
        icon.color = color;
    }
}
}