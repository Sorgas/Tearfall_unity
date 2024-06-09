using game.model.component.unit;
using util.lang.extension;
using static types.unit.UnitStatusEffects;

namespace types.unit.disease {
// Lists all diseases in game. 
public class Diseases {
    public static readonly Disease STARVATION = new ConditionalDisease("starvation", true, 60, "Suffering from not eating for a long time.",
        "{0} is starving!", "skull", unit => unit.take<UnitNeedComponent>().hunger <= 0);

    static Diseases() {
        STARVATION.stages.Add(new DiseaseStage("starvation", 0, 0.6f, MODERATE_STARVATION.name));
        STARVATION.stages.Add(new DiseaseStage("starvation", 0.6f, 0.85f, UnitStatusEffects.STARVATION.name));
        STARVATION.stages.Add(new DiseaseStage("starvation", 0.85f, 1f, EXTREME_STARVATION.name));
        STARVATION.treatments.Add("food");
    }
}
}