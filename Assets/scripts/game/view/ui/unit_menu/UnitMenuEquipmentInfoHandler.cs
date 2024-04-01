using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.view.ui.unit_menu {
// displays equipped items into slot buttons
public class UnitMenuEquipmentInfoHandler : UnitMenuTab {
    public EquipmentSlotHandler headSlot;
    public EquipmentSlotHandler bodySlot;
    public EquipmentSlotHandler handsSlot;
    public EquipmentSlotHandler legsSlot;
    public EquipmentSlotHandler feetSlot;
    public UnitEquipmentSlotLayerHandler rightHandSlot;
    public UnitEquipmentSlotLayerHandler leftHandSlot;

    protected override void updateView() {
        UnitEquipmentComponent equipment = unit.take<UnitEquipmentComponent>();
        fillSlot(headSlot, "head", equipment);
        fillSlot(bodySlot, "body", equipment);
        fillSlot(legsSlot, "legs", equipment);
        fillSlot(feetSlot, "feet", equipment);
        fillSlot(handsSlot, "hands", equipment);
        fillGrabSlot(rightHandSlot, "right hand", equipment);
        fillGrabSlot(leftHandSlot, "left hand", equipment);
    }
    
    private void fillSlot(EquipmentSlotHandler handler, string name, UnitEquipmentComponent equipment) {
        if (equipment.slots.ContainsKey(name)) {
            handler.showSlot(unit, equipment.slots[name]);
        }
    }
    
    private void fillGrabSlot(UnitEquipmentSlotLayerHandler handler, string name, UnitEquipmentComponent equipment) {
        if (equipment.grabSlots.ContainsKey(name)) {
            handler.initFor(unit, equipment.grabSlots[name].item, -1);
        }
    }
}
}