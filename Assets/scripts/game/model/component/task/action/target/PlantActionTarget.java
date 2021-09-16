package stonering.entity.job.action.target;

import stonering.enums.action.ActionTargetTypeEnum;
import stonering.util.geometry.IntVector3;
import stonering.entity.plant.AbstractPlant;

public class PlantActionTarget extends ActionTarget {
    protected AbstractPlant plant;

    public PlantActionTarget(AbstractPlant plant) {
        super(ActionTargetTypeEnum.ANY);
        this.plant = plant;
    }

    @Override
    public IntVector3 getPosition() {
        return plant.position;
    }

    public AbstractPlant getPlant() {
        return plant;
    }

    public void setPlant(AbstractPlant plant) {
        this.plant = plant;
    }
}
