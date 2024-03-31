using System;
using game.model.component.unit;
using game.view.ui.util;
using Leopotam.Ecs;
using TMPro;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.unit_menu {
public class UnitMenuHealthInfoHandler : UnitMenuTab {
    public TextMeshProUGUI statusText;
    public ProgressBarHandler rest;
    public ProgressBarHandler hunger;
    public ProgressBarHandler thirst;
    public Button restPlusButton;
    public Button restMinusButton;
    public Button hungerPlusButton;
    public Button hungerMinusButton;
    public Button thirstPlusButton;
    public Button thirstMinusButton;

    // TODO lists for injures

    public void Start() {
        restPlusButton.onClick.AddListener(() => {
            if (unit == EcsEntity.Null) return;
            ref UnitNeedComponent component = ref unit.takeRef<UnitNeedComponent>();
            component.rest = Math.Min(component.rest + 0.05f, 1f);
        });
        restMinusButton.onClick.AddListener(() => {
            if (unit == EcsEntity.Null) return;
            ref UnitNeedComponent component = ref unit.takeRef<UnitNeedComponent>();
            component.rest = Math.Max(component.rest - 0.05f, 0);
        });
        hungerPlusButton.onClick.AddListener(() => {
            if (unit == EcsEntity.Null) return;
            ref UnitNeedComponent component = ref unit.takeRef<UnitNeedComponent>();
            component.hunger = Math.Min(component.hunger + 0.05f, 1f);
        });
        hungerMinusButton.onClick.AddListener(() => {
            if (unit == EcsEntity.Null) return;
            ref UnitNeedComponent component = ref unit.takeRef<UnitNeedComponent>();
            component.hunger = Math.Max(component.hunger - 0.05f, 0);
        });
        thirstPlusButton.onClick.AddListener(() => {
            if (unit == EcsEntity.Null) return;
            ref UnitNeedComponent component = ref unit.takeRef<UnitNeedComponent>();
            component.thirst = Math.Min(component.thirst + 0.05f, 1f);
        });
        thirstMinusButton.onClick.AddListener(() => {
            if (unit == EcsEntity.Null) return;
            ref UnitNeedComponent component = ref unit.takeRef<UnitNeedComponent>();
            component.thirst = Math.Max(component.thirst - 0.05f, 0);
        });
    }

    protected override void updateView() {
        if (unit == EcsEntity.Null) return;
        HealthComponent component = unit.take<HealthComponent>();
        UnitNeedComponent needs = unit.take<UnitNeedComponent>();
        statusText.text = component.overallStatus;
        rest.setValue(needs.rest);
        hunger.setValue(needs.hunger);
        thirst.setValue(needs.thirst);
    }
}
}