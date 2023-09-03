using System.Collections.Generic;
using game.model.component.unit;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.view.ui.unit_menu {
    public class UnitMenuEquipmentInfoHandler : UnitMenuTab {
        public EquipmentSlotHandler headSlot;
        public EquipmentSlotHandler bodySlot;
        public EquipmentSlotHandler handsSlot;
        public EquipmentSlotHandler legsSlot;
        public EquipmentSlotHandler feetSlot;
        public EquipmentSlotHandler rightHandSlot;
        public EquipmentSlotHandler leftHandSlot;
        private Dictionary<string, EquipmentSlotHandler> slotToViewMap = new();

        public UnitMenuEquipmentInfoHandler() {
            slotToViewMap.Add("head", headSlot);
            slotToViewMap.Add("right hand", headSlot);
            slotToViewMap.Add("left hand", headSlot);
            slotToViewMap.Add("body", headSlot);
            slotToViewMap.Add("legs", headSlot);
            slotToViewMap.Add("feet", headSlot);
        }

        public override void initFor(EcsEntity unit) {
            UnitEquipmentComponent equipment = unit.take<UnitEquipmentComponent>();
            // equipment.slots[].item;

            // HealthComponent component = unit.take<HealthComponent>();
            // UnitNeedComponent needs = unit.take<UnitNeedComponent>();
            // statusText.text = component.overallStatus;
            // sleepNeed.setValue(needs.rest);
            // hunger.setValue(needs.hunger);
            // thirst.setValue(needs.thirst);
        }
    }
}