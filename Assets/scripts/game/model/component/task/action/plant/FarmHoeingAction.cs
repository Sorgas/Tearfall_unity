using game.model.component.task.action.target;
using game.model.util;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionConditionStatusEnum;

// TODO two units can hoe same tile. make farm track targeted tiles
// TODO when finished, should search another tile (make hoeing in pre-action, and tile selection in current action)
namespace game.model.component.task.action.plant {
    /**
     * Action for preparing farm soil for planting plants.
     * Selects tile to hoe within farm and creates FarmTileHoeningAction.
     * Finishes when there is no tiles to hoe.
     *
     * @author Alexander on 20.03.2019.
     */
    public class FarmHoeingAction : Action {
        public readonly FarmHoeingActionTarget farmTarget;
        private EcsEntity farm;

        public FarmHoeingAction(EcsEntity farm) : base(new FarmHoeingActionTarget(farm)) {
            name = "hoeing action";
            farmTarget = (FarmHoeingActionTarget)target;
            this.farm = farm;

            startCondition = () => {
                Vector3Int targetPosition = findTile();
                if (targetPosition == Vector3Int.back) return OK; // no more tiles to hoe, can finish successfully
                lockZoneTile(farm, targetPosition);
                return addPreAction(new FarmTileHoeingAction(targetPosition, farm)); // tile found, create subtask
            };
        }

        // finds tile to hoe
        private Vector3Int findTile() {
            if (task.Has<TaskPerformerComponent>())
                return farmTarget.lookupFreeNearestTile(performer.pos());
            else {
                Debug.LogWarning("hoeing action checked without performer");
                return farmTarget.lookupFreeTile();
            }
        }
    }
}