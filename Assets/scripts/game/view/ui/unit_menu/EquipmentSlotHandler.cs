using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.ui.unit_menu {
// Shows items it each layer of unit's slot 
public class EquipmentSlotHandler : MonoBehaviour {
    public UnitEquipmentSlotLayerHandler wearLayer;
    public UnitEquipmentSlotLayerHandler armorLayer;

    public void showSlot(EcsEntity unit, WearEquipmentSlot slot) {
        wearLayer.initFor(unit, slot.item, -1);
        armorLayer.initFor(unit, slot.armorItem, -1);
    }
}
}