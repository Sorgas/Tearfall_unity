using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
    public class PlantActionTarget : ActionTarget {
        private EcsEntity plant;
        
        public PlantActionTarget(EcsEntity plant) : base(ActionTargetTypeEnum.NEAR) {
            this.plant = plant;
        }

        public override Vector3Int pos => plant.pos();
    }
}