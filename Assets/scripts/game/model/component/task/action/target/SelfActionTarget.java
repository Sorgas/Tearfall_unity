package stonering.entity.job.action.target;

import stonering.entity.Entity;
import stonering.enums.action.ActionTargetTypeEnum;
import stonering.util.geometry.IntVector3;

import static stonering.entity.job.action.target.ActionTargetStatusEnum.READY;

/**
 * Target for actions with no particular target.
 *
 * @author Alexander on 09.02.2020
 */
public class SelfActionTarget extends ActionTarget {

    public SelfActionTarget() {
        super(ActionTargetTypeEnum.ANY);
    }

    @Override
    public IntVector3 getPosition() {
        return null;
    }

    @Override
    public ActionTargetStatusEnum check(Entity performer) {
        return READY;
    }
}
