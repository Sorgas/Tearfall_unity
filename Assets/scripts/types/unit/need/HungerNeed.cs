using enums.action;
using enums.unit.need;

public class HungerNeed : Need {

    public override TaskPriorityEnum getPriority(float value) {
        if (value > 0.5f) return TaskPriorityEnum.NONE;
        if (value > 0.33f) return TaskPriorityEnum.COMFORT;
        if (value > 0) return TaskPriorityEnum.HEALTH_NEEDS;
        return TaskPriorityEnum.SAFETY;
    }
}