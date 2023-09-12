using game.view.ui.util;
using game.view.ui.workbench;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.unit_menu {
// handles display of item equipped in layer of unit's slot
// adds drop button to ItemButtonWithTooltipHandler
public class EquipmentSlotItemIconHandler : ItemButtonWithTooltipHandler {
    public Button dropButton;
    private bool dropButtonEnabled;
    
    public void Start() {
        dropButton.onClick.AddListener(() => Debug.Log("TODO drop equipped item"));
    }

    public override void initFor(EcsEntity item, int amount = -1) {
        base.initFor(item, amount);
        TooltipParentHandler handler = gameObject.GetComponent<TooltipParentHandler>();
        handler.addTooltipObject(dropButton.gameObject, item != EcsEntity.Null);
            // if (decal.Equals("hand")) {
            //     showDecal(IconLoader.get().getSprite("unit_window/empty_hand"), Color.white);
            // }
            // if (decal.Equals("none")) {
            //     showDecal(null, Color.clear);
            // }
    }
    
    private void showDecal(Sprite sprite, Color color) {
        image.sprite = sprite;
        image.color = color;
    }
}
}