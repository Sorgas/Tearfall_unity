using System.Collections.Generic;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit;
using types.unit.disease;
using util;
using util.lang.extension;

namespace game.model.system.unit {
// rolls progress for diseases. applies disease stage effects
public class UnitDiseaseSystem : LocalModelIntervalEcsSystem {
    private static readonly int interval = GameTime.ticksPerMinute * 5;
    public static readonly float delta = ((float)interval) / GameTime.ticksPerHour; // in-game hours of one interval

    public EcsFilter<UnitDiseaseComponent> filter;
    private List<string> toRemoveDiseases = new();
    
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
        List<string> diseases = new(component.diseases.Keys);
        foreach (string diseaseName in component.diseases.Keys) {
            UnitDisease disease = component.diseases[diseaseName];
            if (disease.type is ConditionalDisease) {
                bool condition = ((ConditionalDisease)disease.type).condition.Invoke(unit);
                disease.addProgress(condition ? 1 : -1 * disease.type.progressDelta * updates);
            } else if (disease.type is CausedDisease) {
                CausedDisease causedDisease = (CausedDisease)disease.type;
                if (causedDisease.hoursToHeal > 0) {
                    if (disease.healProgress < 1) {
                        disease.addProgress(causedDisease.progressDelta * updates);
                        disease.addHealProgress(causedDisease.healingDelta * updates);
                    } else {
                        disease.addProgress(-causedDisease.progressDelta * updates);
                    }
                } else {
                    disease.addProgress(causedDisease.progressDelta * updates);
                }
            } else {
                throw new GameException($"disease {disease.type.name} type {disease.type.GetType().Name} not supported");
            }

            DiseaseStage stage = disease.getStage();
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
                toRemoveDiseases.Add(diseaseName);
            }
        }
        if (toRemoveDiseases.Count > 0) {
            foreach (string diseaseName in toRemoveDiseases) {
                removeDisease(unit, diseaseName);
            }
            toRemoveDiseases.Clear();
        }
        if (recalculateEffects) model.unitContainer.statusEffectUtility.recalculate(unit);
    }

    private void removeDisease(EcsEntity unit, string diseaseName) {
        UnitDiseaseComponent component = unit.take<UnitDiseaseComponent>();
        component.diseases.Remove(diseaseName);
        if (component.diseases.Count == 0) {
            unit.Del<UnitDiseaseComponent>();
        }
    }
}
}