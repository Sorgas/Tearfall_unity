using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
    public class ZoneActionTarget : ActionTarget {
        private readonly EcsEntity zone;
        public override Vector3Int pos => zone.take<ZoneComponent>().tiles[0];
            
        public ZoneActionTarget(EcsEntity zone, ActionTargetTypeEnum type) : base(type) {
            this.zone = zone;
        }
    }
}