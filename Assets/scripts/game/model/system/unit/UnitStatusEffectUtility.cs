using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using MoreLinq;
using types.unit;
using util.lang.extension;
using static types.unit.UnitAttributes;
using static types.unit.UnitProperties;

namespace game.model.system.unit {
// Recalculate unit's attributes and properties when active status effects change. 
// Unit's attributes and properties have base values and effective values. Active status effects can change effective values. 
public class UnitStatusEffectUtility {
    // Effects have value offsets, multipliers and bonuses. 
    // Offsets are percentage modifiers, expected to come from many state effects. Offsets are added together before applying.
    // Multipliers are percentage modifiers, expected to come from few effects like age. 
    // Bonuses are flat modifiers, applied after all other modifiers.
    // TODO add attribute -> property bonuses
    public void recalculate(EcsEntity unit) {
        UnitStatusEffectsComponent effectsComponent = unit.take<UnitStatusEffectsComponent>();
        UnitPropertiesComponent propertiesComponent = unit.take<UnitPropertiesComponent>();
        List<UnitStatusEffect> allEffects = new();
        effectsComponent.effects.Select(name => UnitStatusEffects.effects[name]).ForEach(effect => allEffects.Add(effect));
        if (effectsComponent.hungerNeedEffect != null) {
            allEffects.Add(UnitStatusEffects.effects[effectsComponent.hungerNeedEffect]);
        }
        if (effectsComponent.restNeedEffect != null) {
            allEffects.Add(UnitStatusEffects.effects[effectsComponent.restNeedEffect]);
        }
        
        // only bonuses are applied to attributes
        foreach (string attribute in all) {
            UnitIntProperty unitProperty = propertiesComponent.attributes[attribute];
            unitProperty.value = (int) (unitProperty.baseValue + getBonuses(allEffects, attribute));
        }
        foreach (var property in ALL) {
            propertiesComponent.properties[property.name].reset();
        }
        foreach (var property in ALL) {
            float totalOffsets = allEffects
                .Where(effect => effect.offsets.ContainsKey(property.name))
                .Select(effect => effect.offsets[property.name]).Sum();
            float totalMultipliers = allEffects
                .Where(effect => effect.multipliers.ContainsKey(property.name))
                .Select(effect => effect.multipliers[property.name])
                .Aggregate(1f, (value1, value2) => value1 * value2);
            float totalBonuses = allEffects
                .Where(effect => effect.bonuses.ContainsKey(property.name))
                .Select(effect => effect.bonuses[property.name])
                .Aggregate(0, (value1, value2) => value1 + value2);
            UnitFloatProperty unitProperty = propertiesComponent.properties[property.name];
            unitProperty.value = unitProperty.value * (1 + totalOffsets) * totalMultipliers + totalBonuses;
        }
        calculateHealthProperties(propertiesComponent, allEffects);
        calculateGameplayProperties(propertiesComponent, allEffects);
    }

    private void calculateHealthProperties(UnitPropertiesComponent component, List<UnitStatusEffect> effects) {
        // PAIN
        // BLOOD
        // HUNGERRATE
        // THIRSTRATE
        // FATIGUERATE
        float painFactor = 1 - Math.Max(0, component.properties[PAIN.name].value - 0.1f) / 2f;
        component.properties[CONSCIOUSNESS.name].value =
            component.properties[CONSCIOUSNESS.name].baseValue *
            painFactor *
            (1 - (1 - component.properties[BLOOD.name].value) / 2f) *
            (1 - (1 - component.properties[BREATHING.name].value) / 4f) *
            getOffsets(effects, CONSCIOUSNESS.name);
        component.properties[BREATHING.name].value =
            component.properties[BREATHING.name].baseValue *
            getOffsets(effects, BREATHING.name); // TODO add lung efficiency
        // EATING
        // DRINKING
        component.properties[SIGHT.name].value =
            component.properties[SIGHT.name].baseValue *
            getOffsets(effects, SIGHT.name); // TODO add eyes efficiency
        component.properties[MOVESPEED.name].value =
            component.properties[MOVESPEED.name].baseValue *
            component.properties[CONSCIOUSNESS.name].value *
            (1 + (component.attributes[AGILITY].value - 10) * 0.04f) *
            getOffsets(effects, MOVESPEED.name); // TODO add leg effectiveness
        component.properties[WORKSPEED.name].value =
            component.properties[WORKSPEED.name].baseValue *
            (1 - (1 - component.properties[SIGHT.name].value) / 2f) *
            component.properties[CONSCIOUSNESS.name].value *
            getOffsets(effects, WORKSPEED.name);
    }

    private void calculateGameplayProperties(UnitPropertiesComponent component, List<UnitStatusEffect> effects) {
        component.properties[CARRYWEIGHT.name].value =
            component.properties[CARRYWEIGHT.name].baseValue *
            (1 + (component.attributes[STRENGTH].value - 10) * 0.04f) *
            getOffsets(effects, CARRYWEIGHT.name);
    }
    
    private float getOffsets(List<UnitStatusEffect> effects, string property) {
        return 1 + effects.Where(effect => effect.offsets.ContainsKey(property)).Select(effect => effect.offsets[property]).Sum();
    }
    
    private float getBonuses(List<UnitStatusEffect> effects, string property) {
        return 1 + effects.Where(effect => effect.bonuses.ContainsKey(property)).Select(effect => effect.bonuses[property]).Sum();
    }
}
}