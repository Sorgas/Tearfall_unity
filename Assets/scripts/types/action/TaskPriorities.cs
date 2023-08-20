using util.geometry;

namespace types.action {
    public class TaskPriorities {
        public static TaskPriorities instance { get; } = new();
        public const int NONE = 0;                 // task not required.
        public const int TASK_MIN_PRIORITY = 1;
        public const int COMFORT = 3;               // performed, when no job is available
        public const int JOB = 5;                   // default priority for jobs
        public const int HEALTH_NEEDS = 7;          // will stop job for satisfying need
        public const int TASK_MAX_PRIORITY = 8;
        public const int SAFETY = 9;                // avoiding health harm (heavy need level, enemies)
        
        public static readonly IntRange playerRange = new(TASK_MAX_PRIORITY, TASK_MIN_PRIORITY); // priorities available to player for setting to settler job or tasks.
        public static readonly IntRange range = new(SAFETY, TASK_MIN_PRIORITY); // priorities available to player

        private TaskPriorities() { }
    }
}