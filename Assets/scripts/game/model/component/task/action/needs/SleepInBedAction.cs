using enums.action;
using enums.unit.need;
using game.model.component;
using game.model.component.building;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.system;
using Leopotam.Ecs;
using types;
using util.lang.extension;

// when units have medium values of rest need, they will do this action
public class SleepInBedAction : Action {
    private const float baseSleepSpeed = RestNeed.hoursToHealth / RestNeed.hoursToSafety / 8 / GameTime.ticksPerHour;
    private float initialRest;

    public SleepInBedAction(EcsEntity bed) : base(new EntityActionTarget(bed, ActionTargetTypeEnum.EXACT)) {
        startCondition = () => {
            if (model.buildingContainer.get(bed.pos()) == bed 
                    && bed.Has<BuildingSleepFurnitureC>())
                return ActionConditionStatusEnum.OK;
            return ActionConditionStatusEnum.FAIL;
        };
        
        // TODO disable vision, decrease hearing
        onStart = () => {
            initialRest = performer.take<UnitNeedComponent>().rest;
            // Orientations bedOrientation = bed.take<BuildingComponent>().orientation;
            // performer.take<UnitVisualComponent>().handler.rotate(bedOrientation);
            speed = countRestSpeed(bed);
            performer.Replace(new UnitVisualOnBuildingComponent());
        };
        
        progressConsumer = (unit, delta) => {
            ref UnitNeedComponent component = ref performer.takeRef<UnitNeedComponent>();
            component.rest += delta; // decrease fatigue
        };
        
        finishCondition = () => performer.take<UnitNeedComponent>().rest <= 0; // stop sleeping
        
        // TODO restore vision and hearing
        // TODO if fatigue is not restored completely, apply negative mood buff
        onFinish = () => {
            // performer.take<UnitVisualComponent>().handler.rotate(Orientations.N);
            performer.Del<UnitVisualOnBuildingComponent>();
        };
    }

    // TODO consider bed quality and treats
    private float countRestSpeed(EcsEntity bed) {
        QualityEnum bedQuality = bed.Has<QualityComponent>() ? bed.take<QualityComponent>().quality : QualityEnum.AWFUL;
        return baseSleepSpeed * Needs.rest.getSleepSpeedByBedQuality(bedQuality);
    }

    public override float getActionProgress() {
        return 1f - performer.take<UnitNeedComponent>().rest / initialRest;
    }
}