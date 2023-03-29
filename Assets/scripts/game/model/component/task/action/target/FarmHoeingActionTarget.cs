using game.model.util;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {

    // finds unhoed tile 
    public class FarmHoeingActionTarget : ActionTarget {
        private EcsEntity zone;
        private Vector3Int position = Vector3Int.back;

        public FarmHoeingActionTarget(EcsEntity zone) : base(ActionTargetTypeEnum.NEAR) {
            this.zone = zone;
        }

        public override Vector3Int pos {
            get {
                if (position == Vector3Int.back) lookupFreeTile();
                return position;
            }
        }

        public Vector3Int lookupFreeTile() {
            position = ZoneUtils.findUnhoedTile(zone.take<ZoneComponent>(), GameModel.get().currentLocalModel);
            return position;
        }

        public Vector3Int lookupFreeNearestTile(Vector3Int position) {
            position = ZoneUtils.findNearestUnhoedTile(zone.take<ZoneComponent>(), zone.take<ZoneTrackingComponent>(), position, GameModel.get().currentLocalModel);
            return position;
        }
    }
}