package stonering.entity.job.action.target;

import stonering.enums.action.ActionTargetTypeEnum;
import stonering.util.geometry.IntVector3;

/**
 * Targets to some tile.
 */
public class PositionActionTarget extends ActionTarget {
    private IntVector3 targetPosition;

    public PositionActionTarget(IntVector3 targetPosition, ActionTargetTypeEnum placement) {
        super(placement);
        this.targetPosition = targetPosition;
    }

    @Override
    public IntVector3 getPosition() {
        return targetPosition;
    }
}
