//Statues for tasks.
namespace enums.action {
    public enum TaskStatusEnum {
        OPEN,                   // newly created, invalid for assigned tasks
        ACTIVE,                 // taken by performer, invalid for unassigned tasks
        COMPLETE,               // complete (removed from container), invalid for unassigned tasks
        FAILED,                 // not complete (removed from container), invalid for unassigned tasks
        CANCELED                // cancelled by player (removed from container), valid for all
    }
}
