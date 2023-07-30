using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using game.view.system.mouse_tool;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.unit_menu {
    public class UnitMenuGeneralInfoHandler : UnitMenuTab {
        public TextMeshProUGUI nameNicknameProfessionText;
        public TextMeshProUGUI ageText;
        public TextMeshProUGUI healthMoodWealthText;
    
        public Image toolImage1;
        public TextMeshProUGUI toolText1;
    
        public Image toolImage2;
        public TextMeshProUGUI toolText2;
    
        public Image activityImage;
        public TextMeshProUGUI activityText;
        public Button moveToButton;
    
        public override void initFor(EcsEntity unit) {
            UnitComponent unitComponent = unit.take<UnitComponent>();
            nameNicknameProfessionText.text = unit.name(); // TODO add nickname, profession
            ageText.text = "Age: " + unit.take<AgeComponent>().age;
            healthMoodWealthText.text = unit.take<HealthComponent>().overallStatus + ", "
                                                                                   + unit.take<MoodComponent>().status + ", "
                                                                                   + unit.take<OwnershipComponent>().wealthStatus;
            showTools(unit);
            showActivity(unit);
            // TODO if unit is player-controlled
            moveToButton.onClick.AddListener(() => {
                MouseToolManager.get().setUnitMovementTarget(unit);
            });
        }

        // shows images of tools in hands or empty-hand icon
        private void showTools(EcsEntity unit) {
            UnitEquipmentComponent equipment = unit.take<UnitEquipmentComponent>();
            string leftSlot = findSlotName(equipment.grabSlots.Keys, "left");
            string rightSlot = findSlotName(equipment.grabSlots.Keys, "right");
            if(leftSlot != null && rightSlot != null && leftSlot != rightSlot) {
                showSlot(equipment.grabSlots[rightSlot].grabbedItem, toolImage1, toolText1);
                showSlot(equipment.grabSlots[leftSlot].grabbedItem, toolImage2, toolText2);
            } else {
                List<EcsEntity> items = equipment.grabSlots.Values.Where(slot => slot.grabbedItem != EcsEntity.Null)
                    .Select(slot => slot.grabbedItem).Take(2).ToList();
                if(items.Count >= 1) showSlot(items[0], toolImage1, toolText1);
                if(items.Count >= 2) showSlot(items[1], toolImage2, toolText2);
            }
        }

        private void showActivity(EcsEntity unit) {
            if (!unit.Has<UnitCurrentActionComponent>()) return;
            // TODO activity image.
            activityText.text = unit.take<UnitCurrentActionComponent>().action.name;
        }

        private void showSlot(EcsEntity item, Image image, TextMeshProUGUI text) {
            if (item == EcsEntity.Null) {
                image.sprite = IconLoader.get().getSprite("unit_window/empty_hand");
                text.text = "none";
            } else {
                image.sprite = ItemVisualUtil.resolveItemSprite(item);
                text.text = item.name();
            }
        }

        private string findSlotName(IEnumerable<string> names, string substring) {
            return names.Where(name => name.Contains(substring)).FirstOrDefault();
        }
    }
}