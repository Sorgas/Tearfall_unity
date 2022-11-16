namespace enums.action {
    public enum TaskPriorityEnum {
        NONE = -1,           // task not required.
        COMFORT = 3,         // performed, when no job is available
        JOB = 5,             // default priority for jobs
        HEALTH_NEEDS = 7,    // will stop job for satisfying need
        // 8 - max priority for jobs
        SAFETY = 9           // avoiding health harm (heavy need level, enemies)

        // public static final int MAX = 10;
        // public final int VALUE; // numeric value for comparing priorities

        // TaskPriorityEnum(int value) {
            // VALUE = value;
        // }
    }
}