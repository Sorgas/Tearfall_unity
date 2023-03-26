using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
    // action target for zones. Should be updated during action checking.
    // Before this returns tile of zone just for checking reachability
    public class ZoneActionTarget : ActionTarget {
        private readonly EcsEntity zone;
        public Vector3Int tile = Vector3Int.back; // can be set after creation
        public override Vector3Int pos => tile == Vector3Int.back ? zone.take<ZoneComponent>().tiles[0] : tile;

        public ZoneActionTarget(EcsEntity zone, ActionTargetTypeEnum type) : base(type) {
            this.zone = zone;
        }
    }
}