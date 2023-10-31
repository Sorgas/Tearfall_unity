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
    public string type;
    public int material;

    public void initType(string type, bool selected) {
        text.text = type;
        setSelected(selected);
        icon.sprite = ItemTypeMap.get().getSprite(type);
    }
    
    public void initMaterial(string type, int materialId, bool selected) {
        Material_ material = MaterialMap.get().material(materialId);
        text.text = material.name + type;
        setSelected(selected);
        icon.sprite = ItemTypeMap.get().getSprite(type);
        icon.color = material.color;
    }

    public void setSelected(bool value) {
        background.color = value ? UiColorsEnum.backgroundHighlight : UiColorsEnum.background;
    }
}
}