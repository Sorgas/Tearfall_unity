using game.model.component.plant;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.container;
using game.model.util.validation;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.plant {
    public class ChopTreeAction : ToolAction {
        private Vector3Int targetPosition;
        
        public ChopTreeAction(Vector3Int position) : base("chop", new PositionActionTarget(position, ActionTargetTypeEnum.NEAR)) {
            name = "chop tree action";
            targetPosition = position;

            // Checks that tree exists on target position, fails if it doesn't.
            // Checks that performer has chopping tool, creates equipping action if needed.
            startCondition = () => {
                log("Checking " + this);
                if (!checkTree()) return FAIL; // tile still valid
                if (!performer.Has<UnitEquipmentComponent>()) return FAIL;
                if (!performer.take<UnitEquipmentComponent>().toolWithActionEquipped(toolAction))
                    return createActionForGettingTool(); // find tool
                return OK; // tool already equipped
            };

            onFinish = () => {
                log("tree chopping finished at " + targetPosition + " by " + performer.name());
                if (!checkTree()) return;
                PlantContainer container = model.plantContainer;
                EcsEntity plant = container.getPlant(targetPosition);
                if (plant.take<PlantComponent>().type.isTree) container.removePlant(plant, true);
            };
        }

        private bool checkTree() {
            return PlaceValidators.TREE_EXISTS.validate(targetPosition, model);
        }

        private ActionCheckingEnum createActionForGettingTool() {
            log("No tool equipped by performer for chopTreeAction");
            EcsEntity item = model.itemContainer.findingUtil.findNearestItemBySelector(toolSelector, performer.pos());
            if (item == EcsEntity.Null) return FAIL;
            lockEntity(item);
            return addPreAction(new EquipToolItemAction(item));
        }
    }
}