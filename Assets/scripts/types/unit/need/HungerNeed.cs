using game.model.component.task.action;
using game.model.component.task.action.needs;
using game.model.component.unit;
using game.model.localmap;
using game.model.system;
using game.model.system.unit;
using Leopotam.Ecs;
using types.action;
using util.lang.extension;

namespace types.unit.need {
    public class HungerNeed : Need {
        // from full satisfaction
        public const float hoursToComfort = 8f;
        public const float hoursToHealth = 12f;
        public const float hoursToSafety = 24f; // from 1 to 0
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
            if (value > healthThreshold) return "peckish";
            if (value > 0) return "hungry";
            return "starving";
        }
        
        public override Action tryCreateAction(LocalModel model, EcsEntity unit) {
            EcsEntity item = model.itemContainer.findingUtil.findFoodItem(unit.pos());
            if (item == EcsEntity.Null) return null;
            return new EatAction(item);
        }
        
        // TODO add eating of raw food when starving, select best food item available
        public override UnitTaskAssignment createDescriptor(LocalModel model, EcsEntity unit) {
            int hungerPriority = unit.take<UnitNeedComponent>().hungerPriority;
            if (hungerPriority != TaskPriorities.NONE) {
                EcsEntity item = model.itemContainer.findingUtil.findFoodItem(unit.pos());
                if (item != EcsEntity.Null)
                    return new UnitTaskAssignment(item, model.itemContainer.getItemAccessPosition(item), "eat", unit, hungerPriority);
            }
            return null;
        }
    }
}
