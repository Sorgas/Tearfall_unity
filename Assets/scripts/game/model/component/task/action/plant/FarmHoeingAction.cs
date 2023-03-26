using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

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
        private Vector3Int targetPosition;
        private EcsEntity farm;

        public FarmHoeingAction(EcsEntity farm) {
            name = "hoeing action";
            this.farm = farm;
            farmTarget = new FarmHoeingActionTarget(farm);
            target = farmTarget;
            
            startCondition = () => {
                if (findTile()) return createHoeingPreAction(); // tile found, create subtask
                return ActionConditionStatusEnum.OK; // no more tiles to hoe, can finish successfully
            };
        }

        // finds tile to hoe, returns false if there is none
        private bool findTile() {
            targetPosition = task.Has<TaskPerformerComponent>() 
                ? farmTarget.lookupFreeNearestTile(performer.pos()) 
                : farmTarget.lookupFreeTile();
            return targetPosition != Vector3Int.back;
        }

        private ActionConditionStatusEnum createHoeingPreAction() {
            addPreAction(new FarmTileHoeingAction(targetPosition, farm));
            return ActionConditionStatusEnum.NEW;
        }
    }
}
