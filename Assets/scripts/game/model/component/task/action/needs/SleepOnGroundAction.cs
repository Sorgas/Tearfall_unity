using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.system;
using Leopotam.Ecs;
using types;
using types.action;
using types.unit.need;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.needs {
    public class SleepOnGroundAction : Action {
        private const float baseSleepSpeed = RestNeed.hoursToHealth / RestNeed.hoursToSafety / (8 * GameTime.ticksPerHour);
        private float restSpeed;
        private float fatigue;
    
        public SleepOnGroundAction(Vector3Int position) : base(new PositionActionTarget(position, ActionTargetTypeEnum.EXACT)) {
            startCondition = () => ActionCheckingEnum.OK;

            // TODO disable vision decrease hearing
            onStart = () => {
                log("starting sleeping");
                speed = baseSleepSpeed * Needs.rest.getSleepSpeedByBedQuality(QualityEnum.AWFUL);
                performer.take<UnitVisualComponent>().handler.rotate(OrientationUtil.getRandom());
            };
        
            ticksConsumer = delta => {
                EcsEntity performer = this.performer;
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
        
        public override bool validate() => true;
    }
}