//Statues for tasks.
namespace types.action {
    public enum TaskStatusEnum {
        COMPLETE,               // complete (removed from container), invalid for unassigned tasks
        FAILED,                 // not complete (removed from container), invalid for unassigned tasks
        CANCELED                // cancelled by player (removed from container), valid for all
    }
}
