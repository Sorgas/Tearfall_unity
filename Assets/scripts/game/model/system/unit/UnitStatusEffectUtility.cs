using System.Linq;
using game.model.component.unit;
using Leopotam.Ecs;
using types.unit;
using util.lang.extension;

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
        foreach (var property in UnitProperties.all) {
            float totalOffsets = effectsComponent.effects.Values
                .Where(effect => effect.offsets.ContainsKey(property.name))
                .Select(effect => effect.offsets[property.name]).Sum();
            float totalMultipliers = effectsComponent.effects.Values
                .Where(effect => effect.multipliers.ContainsKey(property.name))
                .Select(effect => effect.multipliers[property.name])   
                .Aggregate(1f, (value1, value2) => value1 * value2);
            float totalBonuses = effectsComponent.effects.Values
                .Where(effect => effect.bonuses.ContainsKey(property.name))
                .Select(effect => effect.bonuses[property.name])
                .Aggregate(1f, (value1, value2) => value1 * value2);

            UnitFloatProperty unitProperty = propertiesComponent.properties[property.name];
            unitProperty.value = unitProperty.baseValue * (1 + totalOffsets) * totalMultipliers + totalBonuses;
            if (property.name == UnitProperties.MOVESPEED.name) {
                
            }
        }
    }
}
}