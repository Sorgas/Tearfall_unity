namespace enums.action {
    public enum TaskPriorityEnum {
        NONE = -1,           // task not required.
        COMFORT = 3,         // performed, when no job is available
        JOB = 5,
        HEALTH_NEEDS = 7,    // will stop job for satisfying need
        SAFETY = 1         // avoiding health harm (heavy need level)

        // public static final int MAX = 10;
        // public final int VALUE; // numeric value for comparing priorities

        // TaskPriorityEnum(int value) {
            // VALUE = value;
        // }
    }
}