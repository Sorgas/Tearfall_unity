namespace types.action {
    public enum TaskPriorityEnum {
        NONE = -1,           // task not required.
        TASK_MIN_PRIORITY = 1,
        COMFORT = 3,         // performed, when no job is available
        JOB = 5,             // default priority for jobs
        HEALTH_NEEDS = 7,    // will stop job for satisfying need
        TASK_MAX_PRIORITY = 8,
        SAFETY = 9           // avoiding health harm (heavy need level, enemies)
        // tasks priority range [0; 9]
        // public static final int MAX = 10;
        // public final int VALUE; // numeric value for comparing priorities

        // TaskPriorityEnum(int value) {
            // VALUE = value;
        // }
    }
}