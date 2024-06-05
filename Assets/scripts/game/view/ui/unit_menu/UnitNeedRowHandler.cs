using System;
using game.model.component.unit;
using game.view.ui.util;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.unit_menu {
public class UnitNeedRowHandler : MonoBehaviour {
    public ProgressBarHandler progressBar;
    public Button plusButton;
    public Button minusButton;

    private EcsEntity unit;
    private string need;

    public void Start() {
        plusButton.onClick.AddListener(() => changeNeed(0.05f));
        minusButton.onClick.AddListener(() => changeNeed(-0.05f));
    }

    public void init(EcsEntity unit, string need) {
        this.unit = unit;
        this.need = need;
    }

    public void update() {
        if (unit == EcsEntity.Null) return;
        ref UnitNeedComponent component = ref unit.takeRef<UnitNeedComponent>();
        progressBar.setValue(need switch {
            "rest" => component.rest,
            "hunger" => component.hunger,
            "thirst" => component.thirst,
            _ => 0f
        });
    }

    private void changeNeed(float delta) {
        if (unit == EcsEntity.Null) return;
        ref UnitNeedComponent component = ref unit.takeRef<UnitNeedComponent>();
        if (need == "rest") {
            component.rest = clamp(component.rest + delta);
        } else if (need == "hunger") {
            component.hunger = clamp(component.hunger + delta);
        } else if (need == "thirst") {
            component.thirst = clamp(component.thirst + delta);
        }
    }

    private float clamp(float value) {
        return Math.Min(1f, Math.Max(0f, value));
    }
}
}