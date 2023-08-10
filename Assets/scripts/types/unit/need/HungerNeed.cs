using game.model.component.task.action;
using game.model.component.task.action.needs;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace types.unit.need {
    public class HungerNeed : Need {
        public const float hoursToComfort = 8f;
        public const float hoursToHealth = 12f;
        public const float hoursToSafety = 24f;

        private float comfortThreshold = 1f - hoursToComfort / hoursToSafety;
        private float healthThreshold = 1f - hoursToHealth / hoursToSafety;

        public override int getPriority(float value) {
            if (value > comfortThreshold) return TaskPriorities.NONE;
            if (value > healthThreshold) return TaskPriorities.COMFORT;
            if (value > 0) return TaskPriorities.HEALTH_NEEDS;
            return TaskPriorities.SAFETY;
        }

        public Action tryCreateAction(LocalModel model, EcsEntity unit) {
            // Debug.Log("trying to create eat action");
            EcsEntity item = model.itemContainer.findingUtil.findFoodItem(unit.pos());
            if (item == EcsEntity.Null) return null;
            Debug.Log(item.GetInternalId());
            return new EatAction(item);
        }
    }
}
