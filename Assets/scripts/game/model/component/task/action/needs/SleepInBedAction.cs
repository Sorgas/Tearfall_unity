using enums.action;
using game.model.component.building;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.system;
using Leopotam.Ecs;
using types;
using util.lang.extension;

public class SleepInBedAction : Action {
    private float baseRestSpeed ;
    private float restSpeed;
    private float fatigue;

    private float speed = RestNeed.hoursToHealth / RestNeed.hoursToSafety / 8 / GameTime.hour;

    public SleepInBedAction(EcsEntity bed) : base(new EntityActionTarget(bed, ActionTargetTypeEnum.EXACT)) {
        restSpeed = countRestSpeed();
        startCondition = () => {
            if (model.buildingContainer.get(bed.pos()) == bed
                    && bed.Has<BuildingSleepFurnitureComponent>())
                return ActionConditionStatusEnum.OK;
            return ActionConditionStatusEnum.FAIL;
        };
        
        // TODO disable vision
        // TODO decrease hearing
        onStart = () => {
            fatigue = performer.take<UnitNeedComponent>().rest;
            Orientations bedOrientation = bed.take<BuildingComponent>().orientation;
            performer.take<UnitVisualComponent>().handler.rotate(bedOrientation);
        };
        
        progressConsumer = (unit, delta) => {
            fatigue -= delta; // decrease fatigue
        };
        
        finishCondition = () => fatigue <= 0; // stop sleeping
        
        // TODO restore vision and hearing
        // TODO if fatigue is not restored completely, apply negative mood buff
        onFinish = () => {
            performer.take<UnitVisualComponent>().handler.rotate(bedOrientation);
        };
    }

    private float countRestSpeed() {
//        TreatsAspect aspect = task.performer.getAspect(TreatsAspect.class);
//        if(aspect != null && aspect.treats.contains(TreatEnum.FAST_REST)) speed *= 1.1f; // +10%
//        else if(aspect != null && aspect.treats.contains(TreatEnum.SLOW_REST)) speed *= 0.9f; // +10%
        //TODO consider bed quality and treats
        return 0.003125f; // applied every tick. gives 90 points over 8 hours
    }

    /**
     * Creatures should rest before sleeping.
     * Having bad mood, pain or other negative conditions will prevent sleeping.
     */
    private int getRequiredRestLength() {
        return 0; // TODO
    }

    /**
     * Creatures cannot sleep more than 12 hours. Being sick or having an injury will decrease sleep length.
     * TODO consider sickness and injures.
     */
    private int getMaxSleepLength() {
        return 12 * TimeUnitEnum.HOUR.SIZE * TimeUnitEnum.MINUTE.SIZE; // 12 hours max
    }

    public SleepInBedAction() {
        
    }
}