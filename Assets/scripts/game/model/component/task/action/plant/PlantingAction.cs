using game.model.component.task.action.target;
using game.model.util;
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
        private EcsEntity farm;
        
        public PlantingAction(EcsEntity farm) : base (new ZoneActionTarget(farm, ActionTargetTypeEnum.ANY)) {
            name = "planting action";
            this.farm = farm;
            startCondition = () => {
                Vector3Int tile = getTile();
                if (tile == Vector3Int.back) return OK; // no more tiles to plant
                return addPreAction(new PlantSeedToTileAction(tile, farm));
            };

            onFinish = () => {
                // TODO drop seeds?
            };
        }

        private Vector3Int getTile() {
            return task.Has<TaskPerformerComponent>()
                ? ZoneUtils.findNearestUnplantedTile(farm, performer.pos(), model)
                : ZoneUtils.findUnplantedTile(farm, model);
        } 
    }
}
