using enums.action;
using enums.unit.need;

public class RestNeed : Need {
    private const int hoursTo0 = 36;
    
    public override int getHoursTo0() {
        return hoursTo0;
    }

    public override TaskPriorityEnum getPriority(float value) {
        if (value > 0.5f) return TaskPriorityEnum.NONE;
        if (value > 0.32f) return TaskPriorityEnum.COMFORT;
        if (value > 0.32f) return TaskPriorityEnum.HEALTH_NEEDS;
        return TaskPriorityEnum.SAFETY;
    }
}