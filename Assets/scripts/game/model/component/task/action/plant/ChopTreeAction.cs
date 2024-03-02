using game.model.component.plant;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.container;
using game.model.util.validation;
using generation.item;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.plant {
public class ChopTreeAction : ToolAction {

    public ChopTreeAction(EcsEntity tree) : base("chop", new PlantActionTarget(tree, ActionTargetTypeEnum.NEAR)) {
        name = "chop tree action";
        animation = "woodcutting";
        
        // Checks that tree exists on target position, fails if it doesn't.
        // Checks that performer has chopping tool, creates equipping action if needed.
        startCondition = () => {
            log("Checking " + this);
            if (!validate()) return FAIL; // tree exists
            if (!performer.Has<UnitEquipmentComponent>()) return FAIL;
            if (!equipment.toolWithActionEquipped(toolAction))
                return createActionForGettingTool(); // find tool
            return OK; // tool already equipped
        };

        onStart = () => {
            maxProgress = 500;
        };
        
        onFinish = () => {
            Vector3Int targetPosition = target.pos;
            log("tree chopping finished at " + targetPosition + " by " + performer.name());
            if (!validate()) return; // tree can die during chopping
            ItemGenerator generator = new();
            EcsEntity item = model.createEntity();
            PlantContainer container = model.plantContainer;
            EcsEntity plant = container.getPlant(targetPosition);
            generator.generatePlantProduct(model.plantContainer.getBlock(targetPosition), item);
            if (plant.take<PlantComponent>().type.isTree) container.removePlant(plant, true);
            model.itemContainer.onMap.putItemToMap(item, targetPosition);
        };
    }

    private ActionCheckingEnum createActionForGettingTool() {
        log("No tool equipped by performer for chopTreeAction");
        EcsEntity item = model.itemContainer.findingUtil.findNearestItemBySelector(toolSelector, performer.pos());
        if (item == EcsEntity.Null) return FAIL;
        lockEntity(item);
        return addPreAction(new EquipToolItemAction(item));
    }

    public override bool validate() {
        return PlaceValidators.TREE_EXISTS.validate(target.pos, model);
    }
}
}