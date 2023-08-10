using game.model.component.item;
using game.view.ui.workbench;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.unit_menu {
public class EquipmentSlotHandler : MonoBehaviour {
    public Image itemImage;
    public TextMeshProUGUI quantityText;
    public ItemButtonWithTooltipHandler buttonHandler;

    public void showItem(EcsEntity item, int amount) {
        buttonHandler.initFor(item.take<ItemComponent>(), amount);
    }

    public void showEmpty(string decal) {
        quantityText.text = "";
        if (decal.Equals("hand")) {
            itemImage.sprite = IconLoader.get().getSprite("unit_window/empty_hand");
        }
    }
}
}