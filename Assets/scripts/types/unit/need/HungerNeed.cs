using game.model.component.task.action;
using game.model.component.task.action.needs;
using game.model.component.unit;
using game.model.localmap;
using game.model.system;
using game.model.system.unit;
using Leopotam.Ecs;
using types.action;
using types.unit.disease;
using util.lang.extension;

namespace types.unit.need {
// Describes unit's need for food. Need has value [0..1] where 1 means satisfied.
// Depending on value, units get different status effects and seek food with different priority. 
    public class HungerNeed : Need {
        // from full satisfaction
        private const float hoursToComfort = 8f;
        private const float hoursToHealth = 12f;
        private const float hoursToSafety = 24f; // from 1 to 0
        public const float perTickChange = 1f / hoursToSafety / GameTime.ticksPerHour;

        // need values of priority change
        private float comfortThreshold = 1f - hoursToComfort / hoursToSafety; // 0.75 
        private float healthThreshold = 1f - hoursToHealth / hoursToSafety; // 0.5

        public override int getPriority(float value) {
            if (value > comfortThreshold) return TaskPriorities.NONE;
            if (value > healthThreshold) return TaskPriorities.COMFORT;
            if (value > 0) return TaskPriorities.HEALTH_NEEDS;
            return TaskPriorities.SAFETY;
        }

        public override string getStatusEffect(float value) {
            if (value > comfortThreshold) return null;
            if (value > healthThreshold) return UnitStatusEffects.PECKISH.name;
            if (value > 0) return UnitStatusEffects.HUNGRY.name;
            return UnitStatusEffects.RAVENOUSLY_HUNGRY.name;
        }
        
        public override Action tryCreateAction(LocalModel model, EcsEntity unit) {
            int minimumFoodQuality = getMinimumFoodQualityForUnit(unit);
            EcsEntity item = model.itemContainer.consumableItemsFindingUtil.findFoodItem(unit.pos(), minimumFoodQuality);
            if (item == EcsEntity.Null) return null;
            return new EatAction(item);
        }
        
        // TODO add eating of raw food when starving, select best food item available when not starving, select nearest item when starving
        // TODO add consumption policies for settlers
        public override UnitTaskAssignment tryCreateAssignment(LocalModel model, EcsEntity unit) {
            int hungerPriority = unit.take<UnitNeedComponent>().hungerPriority;
            
            if (hungerPriority != TaskPriorities.NONE) {
                int minimumFoodQuality = getMinimumFoodQualityForUnit(unit);
                EcsEntity item = model.itemContainer.consumableItemsFindingUtil.findFoodItem(unit.pos(), minimumFoodQuality);
                if (item != EcsEntity.Null)
                    return new UnitTaskAssignment(item, model.itemContainer.getItemAccessPosition(item), "eat", unit, hungerPriority);
            }
            return null;
        }

        // TODO eat only prepared food when lightly hungry, 1 when hungry, from 2 to 5 when starving
        private int getMinimumFoodQualityForUnit(EcsEntity unit) {
            return FoodQualities.RAW_MEAT;
        }
        
        public override UnitDisease createDisease() {
            return new UnitDisease(Diseases.STARVATION);
        }
    }

public class FoodQualities {
    public const int MEAL = 2; // prepared fresh dishes
    public const int UNPREPARED = 1; // raw vegetables
    public const int RAW_MEAT = 0; // raw meat
    public const int CORPSE = -1; // animal corpses
    public const int SAPIENT_MEAT = -2; // sapient creature meat
    public const int SAPIENT_CORPSE = -3; // sapient creature corpse
}
}
