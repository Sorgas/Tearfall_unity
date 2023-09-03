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
    public EquipmentSlotHandler rightHandSlot;
    public EquipmentSlotHandler leftHandSlot;

    public override void initFor(EcsEntity unit) {
        UnitEquipmentComponent equipment = unit.take<UnitEquipmentComponent>();
        fillSlot(headSlot, "head", equipment);
        fillSlot(bodySlot, "body", equipment);
        fillSlot(legsSlot, "legs", equipment);
        fillSlot(feetSlot, "feet", equipment);
        fillSlot(handsSlot, "hands", equipment);
        fillGrabSlot(rightHandSlot, "right hand", equipment);
        fillGrabSlot(leftHandSlot, "left hand", equipment);
        Debug.Log("initing equipment");
    }

    private void fillSlot(EquipmentSlotHandler handler, string name, UnitEquipmentComponent equipment) {
        if (equipment.slots.ContainsKey(name)) {
            handler.showSlot(equipment.slots[name].item, "none");
        }
    }
    
    private void fillGrabSlot(EquipmentSlotHandler handler, string name, UnitEquipmentComponent equipment) {
        if (equipment.grabSlots.ContainsKey(name)) {
            handler.showSlot(equipment.grabSlots[name].item, "hand");
        }
    }
}
}