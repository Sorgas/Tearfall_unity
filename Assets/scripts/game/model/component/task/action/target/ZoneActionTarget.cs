using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
    // action target for zones.
    // Should be updated during action checking. Before this, returns tile of zone just for checking reachability
    public class ZoneActionTarget : DynamicActionTarget {
        private readonly EcsEntity zone;
        public Vector3Int tile = Vector3Int.back; // should be set by action during conditions checking
        public override Vector3Int pos => getPosition();

        public ZoneActionTarget(EcsEntity zone, ActionTargetTypeEnum type) : base(type) {
            this.zone = zone;
        }
        
        private Vector3Int getPosition() {
            return tile != Vector3Int.back ? tile : zone.take<ZoneComponent>().tiles[0];
        }

        public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
            return zone.take<ZoneComponent>().tiles;
        }
    }
}