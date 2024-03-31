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
    public EquipmentSlotItemIconHandler rightHandSlot;
    public EquipmentSlotItemIconHandler leftHandSlot;

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
    
    public override void showUnit(EcsEntity unit) {
    }

    private void fillSlot(EquipmentSlotHandler handler, string name, UnitEquipmentComponent equipment) {
        if (equipment.slots.ContainsKey(name)) {
            handler.showSlot(equipment.slots[name]);
        }
    }
    
    private void fillGrabSlot(EquipmentSlotItemIconHandler handler, string name, UnitEquipmentComponent equipment) {
        if (equipment.grabSlots.ContainsKey(name)) {
            handler.initFor(equipment.grabSlots[name].item, -1);
        }
    }

    // immediately drops items from unit
    private void handleDropItemButton() {
        
    }
}
}