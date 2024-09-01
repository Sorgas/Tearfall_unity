using game.model.component.building;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.system;
using game.view.ui.debug_tools;
using Leopotam.Ecs;
using types;
using types.action;
using types.item;
using types.unit.need;
using UnityEngine;
using util.lang.extension;

// when units have medium values of rest need, they will do this action
namespace game.model.component.task.action.needs {
    public class SleepInBedAction : Action {
        private EcsEntity bed;
        private const float baseSleepSpeed = RestNeed.hoursToHealth / RestNeed.hoursToSafety / 8 / GameTime.ticksPerHour;
        private float initialRest;

        public SleepInBedAction(EcsEntity bed) : base(new BuildingActionTarget(bed, ActionTargetTypeEnum.EXACT)) {
            this.bed = bed;
            startCondition = () => {
                if (model.buildingContainer.getBuilding(bed.pos()) == bed 
                    && bed.Has<BuildingSleepFurnitureC>())
                    return ActionCheckingEnum.OK;
                return ActionCheckingEnum.FAIL;
            };
        
            // TODO disable vision, decrease hearing
            onStart = () => {
                initialRest = performer.take<UnitNeedComponent>().rest;
                // Orientations bedOrientation = bed.take<BuildingComponent>().orientation;
                // performer.take<UnitVisualComponent>().handler.rotate(bedOrientation);
                speed = countRestSpeed(bed);
                performer.Replace(new UnitVisualOnBuildingComponent());
            };
        
            ticksConsumer = delta => {
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
            ItemQuality bedQuality = bed.Has<QualityComponent>() ? ItemQualities.get(bed.take<QualityComponent>().quality) : ItemQualities.AWFUL;
            return baseSleepSpeed * bedQuality.sleepSpeedMod;
        }

        public override bool validate() {
            if (!bed.Has<BuildingSleepFurnitureC>()) {
                Debug.LogWarning($"{name}: target building is not bed");
                return false;
            }
            return true;
        }
    }
}