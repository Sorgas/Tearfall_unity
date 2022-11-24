using enums.action;
using enums.unit.need;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.system;
using types;
using UnityEngine;
using util.lang.extension;

public class SleepOnGroundAction : Action {
    private const float baseSleepSpeed = RestNeed.hoursToHealth / RestNeed.hoursToSafety / 8 / GameTime.hour;
    private float restSpeed;
    private float fatigue;
    
    public SleepOnGroundAction(Vector3Int position) : base(new PositionActionTarget(position, ActionTargetTypeEnum.EXACT)) {
        startCondition = () => ActionConditionStatusEnum.OK;

        // TODO disable vision decrease hearing
        onStart = () => {
            log("starting sleeping");
            speed = baseSleepSpeed * Needs.rest.getSleepSpeedByBedQuality(QualityEnum.AWFUL);
            performer.take<UnitVisualComponent>().handler.rotate(OrientationUtil.getRandom());
        };
        
        progressConsumer = (unit, delta) => {
            ref UnitNeedComponent component = ref performer.takeRef<UnitNeedComponent>();
            component.rest -= delta; // decrease fatigue
        };
        
        finishCondition = () => performer.take<UnitNeedComponent>().rest <= 0; // stop sleeping
        
        // TODO restore vision and hearing
        // TODO if fatigue is not restored completely, apply negative mood buff
        onFinish = () => {
            log("finishing sleeping");
            performer.take<UnitVisualComponent>().handler.rotate(Orientations.N);
        };
    }
}