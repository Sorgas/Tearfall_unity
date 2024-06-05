using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using game.view.ui.util;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using util.lang.extension;

namespace game.view.ui.unit_menu {
public class UnitMenuHealthInfoHandler : UnitMenuTab {
    public TextMeshProUGUI statusText;
    public UnitNeedRowHandler restNeed;
    public UnitNeedRowHandler hungerNeed;
    public UnitNeedRowHandler thirstNeed;
    public RectTransform diseaseColumn;

    private readonly Dictionary<string, DiseaseRowHandler> diseaseRows = new();

    // TODO lists for injures
    // TODO use ModelUpdateEvent to change need values

    public override void showUnit(EcsEntity unit) {
        base.showUnit(unit);
        restNeed.init(unit, "rest");
        hungerNeed.init(unit, "hunger");
        thirstNeed.init(unit, "thirst");
    }

    protected override void updateView() {
        UnitHealthComponent component = unit.take<UnitHealthComponent>();
        UnitNeedComponent needs = unit.take<UnitNeedComponent>();
        // statusText.text = component.overallStatus;
        restNeed.update();
        hungerNeed.update();
        thirstNeed.update();
        updateDiseaseColumn();
    }

    private void updateDiseaseColumn() {
        if (!unit.Has<UnitDiseaseComponent>()) return;
        UnitDiseaseComponent component = unit.take<UnitDiseaseComponent>();
        bool changed = false;
        List<string> sortedDiseases = component.diseases.Keys.OrderBy(s => s).ToList();
        foreach (string diseaseName in sortedDiseases) {
            if (!diseaseRows.Keys.Contains(diseaseName)) {
                GameObject row = PrefabLoader.create("UnitDiseaseRow", diseaseColumn, Vector3.zero);
                row.name = diseaseName + "Row";
                DiseaseRowHandler handler = row.GetComponent<DiseaseRowHandler>();
                diseaseRows.Add(diseaseName, handler);
                handler.init(component.diseases[diseaseName]);
                changed = true;
            }
            diseaseRows[diseaseName].init(component.diseases[diseaseName]);
        }
        List<string> toRemove = diseaseRows.Keys
            .Where(key => !component.diseases.ContainsKey(key)).ToList();
        changed = changed || toRemove.Count > 0;
        toRemove.ForEach(key => {
            Destroy(diseaseRows[key]);
            diseaseRows.Remove(key);
        });
        if (changed) {
            int count = 0;
            foreach (var key in diseaseRows.Keys) {
                diseaseRows[key].gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
}