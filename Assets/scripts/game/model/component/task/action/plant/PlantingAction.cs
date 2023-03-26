using game.model.component.task.action.target;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionConditionStatusEnum;

namespace game.model.component.task.action.plant { 
    /**
     * Action for planting a farm.
     * Checks if there is some unplanted tile, then creates action to plant it.
     */
    public class PlantingAction : Action {
        private EcsEntity zone;
        
        public PlantingAction(EcsEntity zone) : base (new ZoneActionTarget(zone, ActionTargetTypeEnum.ANY)) {
            name = "planting action";
            this.zone = zone;
            startCondition = () => {
                Vector3Int tile = getTile();
                if (tile == Vector3Int.back) return OK; // no more tiles to plant
                return addPreAction(new PlantSeedToTileAction(tile, zone));
            };

            onFinish = () => {
                // TODO drop seeds?
            };
        }

        private Vector3Int getTile() {
            return zone.take<ZoneTrackingComponent>().tiles[ZoneTaskTypes.PLANT].firstOrDefault(Vector3Int.back);
        } 
    }
}
