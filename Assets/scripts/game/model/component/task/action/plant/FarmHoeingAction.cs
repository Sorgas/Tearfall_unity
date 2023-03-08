using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.item;
using util.lang.extension;

// TODO two units can hoe same tile. make farm track hoed tiles
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
        
        public FarmHoeingAction(EcsEntity zone) {
            farmTarget = new FarmHoeingActionTarget(zone);
            target = farmTarget;
            
            startCondition = () => {
                if (findTile()) return createHoeingPreAction();
                return ActionConditionStatusEnum.OK; // no more tiles to hoe, can finish successfully
            };

            onStart = () => maxProgress = 200;

            onFinish = () => {
                model.localMap.blockType.set(targetPosition, BlockTypes.FARM);
            };
        }

        // finds tile to hoe, returns false if there is none
        private bool findTile() {
            targetPosition = farmTarget.lookupFreeTile();
            return targetPosition != Vector3Int.back;
        }

        private ActionConditionStatusEnum createHoeingPreAction() {
            addPreAction(new FarmTileHoeingAction(targetPosition));
            return ActionConditionStatusEnum.NEW;
        }
    }
}
