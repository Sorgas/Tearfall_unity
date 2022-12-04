using enums.action;
using enums.unit.need;
using game.model.component.task.action;
using Leopotam.Ecs;

public class HungerNeed : Need {
    public const int hoursToComfort = 8;
    public const int hoursToHealth = 12;
    public const int hoursToSafety = 24;
    

    public override TaskPriorityEnum getPriority(float value) {
        if (value > 0.5f) return TaskPriorityEnum.NONE;
        if (value > 0.33f) return TaskPriorityEnum.COMFORT;
        if (value > 0) return TaskPriorityEnum.HEALTH_NEEDS;
        return TaskPriorityEnum.SAFETY;
    }

    public Action tryCreateAction(LocalModel model, EcsEntity entity) {
        
        return null;
    }
}
