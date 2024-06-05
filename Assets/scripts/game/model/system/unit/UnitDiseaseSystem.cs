using System;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit;
using types.unit.disease;
using util.lang.extension;

namespace game.model.system.unit {
// rolls progress for diseases. applies disease stage effects
public class UnitDiseaseSystem : LocalModelIntervalEcsSystem {
    private static readonly int interval = GameTime.ticksPerMinute * 5;
    public static readonly float delta = ((float) interval) / GameTime.ticksPerHour; // in-game hours of one interval

    public EcsFilter<UnitDiseaseComponent> filter;

    public UnitDiseaseSystem() : base(interval) { }
    
    protected override void runLogic(int updates) {
        foreach (var i in filter) {
            increaseDiseaseProgresss(filter.GetEntity(i), filter.Get1(i), updates);
        }
    }

    private void increaseDiseaseProgresss(EcsEntity unit, UnitDiseaseComponent component, int updates) {
        bool recalculateEffects = false;
        UnitPropertiesComponent propertiesComponent = unit.take<UnitPropertiesComponent>();
        float diseaseResistance = propertiesComponent.properties[UnitProperties.DISEASERESIST.name].value;
        foreach (UnitDisease disease in component.diseases.Values) {
            DiseaseStage stage = disease.getStage();
            if (disease.type.hoursToHeal > 0) {
                if (disease.healProgress < 1) {
                    disease.addProgress(disease.type.progressDelta * updates);
                    disease.addHealProgress(disease.type.healingDelta * updates);
                } else {
                    disease.addProgress(-disease.type.progressDelta * updates);
                }
            } else {
                disease.addProgress(disease.type.progressDelta * updates);
            }
            DiseaseStage newStage = disease.getStage();
            if (stage.name != newStage.name) {
                ref UnitStatusEffectsComponent effectsComponent = ref unit.takeRef<UnitStatusEffectsComponent>();
                effectsComponent.effects.Remove(stage.effect);
                effectsComponent.effects.Add(newStage.effect);
                recalculateEffects = true;
            }
            if (disease.progress >= 1 && disease.type.lethal) {
                // TODO kill unit
            }
            if (disease.progress <= 0) {
                removeDisease(unit, disease.type.name);
            }
        }
        if (recalculateEffects) model.unitContainer.statusEffectUtility.recalculate(unit);
    }

    private void removeDisease(EcsEntity unit, string diseaseName) {
        
    }
}
}