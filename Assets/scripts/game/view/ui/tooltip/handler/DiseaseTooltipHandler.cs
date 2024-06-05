using System.Linq;
using game.model.component.unit;
using TMPro;
using types.unit;
using types.unit.disease;
using Unity.Mathematics;

namespace game.view.ui.tooltip.handler {
public class DiseaseTooltipHandler : AbstractTooltipHandler {
    public TextMeshProUGUI title;
    public TextMeshProUGUI progress;
    public TextMeshProUGUI treatment;
    public TextMeshProUGUI description;
    
    
    public override void init(InfoTooltipData newData) {
        base.init(newData);
        UnitDisease disease = (UnitDisease)newData.o;
        title.text = disease.type.name;
        progress.text = $"Progress: {(disease.progress * 100):##.##}%, time to terminal stage: {getKillTime(disease):##.#} hours \n";
        DiseaseStage stage = getStage(disease);
        UnitStatusEffect effect = UnitStatusEffects.effects[stage.effect];
        
        foreach (string key in effect.offsets.Keys) {
            int offsetValue = (int)math.round(effect.offsets[key] * 100);
            progress.text += $"{key}: {offsetValue:+###;-###}% \n";
        }
        foreach (string key in effect.multipliers.Keys) {
            progress.text += $"{key}: {(effect.multipliers[key]):+###.##;-###.##}% \n";
        }
        foreach (string key in effect.bonuses.Keys) {
            progress.text += $"{key}: {(effect.bonuses[key]):+###;-###} \n";
        }
        if (disease.healProgress < 1) {
            treatment.text = $"Time to heal: {getHealTime(disease):##.#} hours \n";
        } else {
            treatment.text = "\n";
        }
        treatment.text += $"Treatment: {disease.type.treatments.Aggregate((s1 ,s2) => $"{s1}, {s2}")}";
        description.text = disease.type.description;
    }

    protected override void closeTooltip() {
        Destroy(gameObject);
    }

    private float getKillTime(UnitDisease disease) {
        return disease.type.hoursToKill * (1 - disease.progress);
    }

    private float getHealTime(UnitDisease disease) {
        return disease.type.hoursToHeal * (1 - disease.healProgress);
    }

    private DiseaseStage getStage(UnitDisease disease) {
        return disease.type.getStage(disease.progress);
    }
}
}