//This values are used when checking action targets (whether creature is in position for performing action).
namespace game.model.component.task.action.target {
    public enum ActionTargetStatusEnum {
        READY, // target position reached
        WAIT, // target position not reached
        STEP_OFF // creature should free target tile
    }
}
