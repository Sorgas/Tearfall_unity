using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using game.view.system.mouse_tool;
using Leopotam.Ecs;
using TMPro;
using types.unit;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;
using static game.view.ui.UiColorsEnum;

namespace game.view.ui.unit_menu {
public class UnitMenuGeneralInfoHandler : UnitMenuTab {
    public TextMeshProUGUI nameNicknameProfessionText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI healthMoodWealthText;

    public UnitEquipmentSlotLayerHandler slotHandler1;
    public TextMeshProUGUI toolText1;

    public UnitEquipmentSlotLayerHandler slotHandler2;
    public TextMeshProUGUI toolText2;

    public Image activityImage;
    public TextMeshProUGUI activityText;
    public Button moveToButton;

    public TextMeshProUGUI moodText;

    public void Start() {
        moveToButton.onClick.AddListener(() => { MouseToolManager.get().setUnitMovementTarget(unit); });
    }

    protected override void updateView() {
        healthMoodWealthText.text =
            unit.take<UnitHealthComponent>().overallStatus + ", " +
            unit.take<OwnershipComponent>().wealthStatus;
        showTools(unit);
        showActivity(unit);
        displayUnitMood(unit);
    }

    public override void showUnit(EcsEntity unit) {
        base.showUnit(unit);
        displayUnitName(unit);
        ageText.text = "Age: " + unit.take<AgeComponent>().age;
    }

    // shows images of tools in hands or empty-hand icon
    private void showTools(EcsEntity unit) {
        UnitEquipmentComponent equipment = unit.take<UnitEquipmentComponent>();
        string leftSlot = findSlotName(equipment.grabSlots.Keys, "left");
        string rightSlot = findSlotName(equipment.grabSlots.Keys, "right");
        if (leftSlot != null && rightSlot != null && leftSlot != rightSlot) {
            showSlot(equipment.grabSlots[rightSlot].item, slotHandler1, toolText1);
            showSlot(equipment.grabSlots[leftSlot].item, slotHandler2, toolText2);
        } else {
            List<EcsEntity> items = equipment.grabSlots.Values.Where(slot => slot.item != EcsEntity.Null)
                .Select(slot => slot.item).Take(2).ToList();
            if (items.Count >= 1) showSlot(items[0], slotHandler1, toolText1);
            if (items.Count >= 2) showSlot(items[1], slotHandler2, toolText2);
        }
    }

    private void showActivity(EcsEntity unit) {
        if (!unit.Has<UnitCurrentActionComponent>()) return;
        // TODO activity image.
        activityText.text = unit.take<UnitCurrentActionComponent>().action.name;
    }

    private void showSlot(EcsEntity item, UnitEquipmentSlotLayerHandler slotHandler, TextMeshProUGUI text) {
        slotHandler.initFor(unit, item, -1);
        if (item == EcsEntity.Null) {
            text.text = "none";
        } else {
            text.text = item.name();
        }
    }

    private string findSlotName(IEnumerable<string> names, string substring) {
        return names.Where(name => name.Contains(substring)).FirstOrDefault();
    }

    private void displayUnitName(EcsEntity unit) {
        string name = unit.name();
        if (unit.Has<UnitNamesComponent>()) {
            UnitNamesComponent namesComponent = unit.take<UnitNamesComponent>();
            if (namesComponent.professionName != null) {
                name += " " + namesComponent.professionName;
            }
            if (namesComponent.nickName != null) {
                name += $" '{namesComponent.nickName}'";
            }
        }
        nameNicknameProfessionText.text = name;
    }

    private void displayUnitMood(EcsEntity unit) {
        float mood = unit.take<UnitPropertiesComponent>().properties[UnitProperties.MOOD.name].value;
        moodText.text = mood.ToString("0.00");
        if (mood < -10) moodText.color = RED_BRIGHT;
        else if (mood < 0) moodText.color = Color.Lerp(RED_BRIGHT, FONT_COLOR, (mood + 10) / 10f);
        if (mood < 10) moodText.color = FONT_COLOR;
        else if (mood < 30) moodText.color = Color.Lerp(FONT_COLOR, GREEN_BRIGHT, (mood - 10) / 20f);
        else moodText.color = GREEN_BRIGHT;
    }
}
}