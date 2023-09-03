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
    public TextMeshProUGUI quantityText;
    public ItemButtonWithTooltipHandler handler;
    public Button dropButton;
    private bool dropButtonEnabled;
    private EcsEntity item;
    
    public void Start() {
        dropButton.onClick.AddListener(() => Debug.Log("TODO drop equipped item"));
    }
    
    public void showSlot(EcsEntity item, string decal) {
        this.item = item;
        if (item == EcsEntity.Null) {
            dropButtonEnabled = false;
            handler.tooltipEnabled = false;
            quantityText.text = "";
            if (decal.Equals("hand")) {
                showDecal(IconLoader.get().getSprite("unit_window/empty_hand"), Color.white);
            }
            if (decal.Equals("none")) {
                showDecal(null, Color.clear);
            }
        } else {
            dropButtonEnabled = true;
            handler.tooltipEnabled = true;
            handler.initFor(item.take<ItemComponent>());
        }
    }

    private void showDecal(Sprite sprite, Color color) {
        handler.image.sprite = sprite;
        handler.image.color = color;
    }

    // public void OnPointerEnter(PointerEventData eventData) {
    //     dropButton.gameObject.SetActive(dropButtonEnabled);
    //     Debug.Log("in");
    // }
    //
    // public void OnPointerExit(PointerEventData eventData) {
    //     dropButton.gameObject.SetActive(false);
    //     Debug.Log("out");
    // }
}
}