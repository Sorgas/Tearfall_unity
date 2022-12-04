using enums.action;
using enums.unit.need;
using game.model.component.task.action;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

public class HungerNeed : Need {
    public const float hoursToComfort = 8f;
    public const float hoursToHealth = 12f;
    public const float hoursToSafety = 24f;

    private float comfortThreshold = 1f - hoursToComfort / hoursToSafety;
    private float healthThreshold = 1f - hoursToHealth / hoursToSafety;

    public override TaskPriorityEnum getPriority(float value) {
        if (value > comfortThreshold) return TaskPriorityEnum.NONE;
        if (value > healthThreshold) return TaskPriorityEnum.COMFORT;
        if (value > 0) return TaskPriorityEnum.HEALTH_NEEDS;
        return TaskPriorityEnum.SAFETY;
    }

    public Action tryCreateAction(LocalModel model, EcsEntity unit) {
        Debug.Log("trying to create eat action");
        EcsEntity item = model.itemContainer.util.findFoodItem(unit.pos());
        if (item == EcsEntity.Null) return null;
        Debug.Log(item.GetInternalId());
        return new EatAction(item);
    }
}
