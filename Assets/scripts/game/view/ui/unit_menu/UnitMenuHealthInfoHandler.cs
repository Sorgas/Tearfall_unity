using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using MoreLinq;
using TMPro;
using types.unit.disease;
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
    private readonly Dictionary<string, UnitDisease> emptyMap = new();
    
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
        Dictionary<string, UnitDisease> unitDiseases = getUnitDiseases(unit);
        bool changed = false;
        // add missing
        unitDiseases.Where(pair => !diseaseRows.ContainsKey(pair.Key)).ForEach(pair => {
            GameObject row = PrefabLoader.create("UnitDiseaseRow", diseaseColumn, Vector3.zero);
            string diseaseName = pair.Key;
            row.name = diseaseName + "Row";
            DiseaseRowHandler handler = row.GetComponent<DiseaseRowHandler>();
            diseaseRows.Add(diseaseName, handler);
            handler.init(pair.Value);
            changed = true;
        });
        // remove 
        diseaseRows.Keys.Where(diseaseName => !unitDiseases.Keys.Contains(diseaseName)).ToList().ForEach(diseaseName => {
            Destroy(diseaseRows[diseaseName].gameObject);
            diseaseRows.Remove(diseaseName);
            changed = true;
        });
        // rearrange positions
        if (changed) {
            int count = 0;
            foreach (var key in diseaseRows.Keys) {
                diseaseRows[key].gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    private Dictionary<string, UnitDisease> getUnitDiseases(EcsEntity unit) {
        return unit.Has<UnitDiseaseComponent>() ? unit.take<UnitDiseaseComponent>().diseases : emptyMap;
    }
}
}