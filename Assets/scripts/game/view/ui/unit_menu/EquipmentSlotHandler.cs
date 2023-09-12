using game.model.component.unit;
using UnityEngine;

namespace game.view.ui.unit_menu {
// handles display of units equipment slot. 
public class EquipmentSlotHandler : MonoBehaviour {
    public EquipmentSlotItemIconHandler wearLayer;
    public EquipmentSlotItemIconHandler armorLayer;

    public void showSlot(WearEquipmentSlot slot) {
        wearLayer.initFor(slot.item, -1);
        armorLayer.initFor(slot.armorItem, -1);
    }
}
}