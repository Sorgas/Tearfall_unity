using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.util;
using Leopotam.Ecs;
using types.action;
using types.unit.skill;
using UnityEngine;
using util.item;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.plant.farm {
// actually hoes one tile of a farm. See FarmHoeingAction
// also destroys plant and substrate on tile
public class FarmTileHoeingAction : ToolAction {
    private const string TOOL_ACTION_NAME = "hoe";
    private EcsEntity zone;

    public FarmTileHoeingAction(Vector3Int tile, EcsEntity zone) : base(TOOL_ACTION_NAME, new FarmTileHoeingActionTarget(tile)) {
        name = "tile hoeing action";
        this.zone = zone;
        usedSkill = UnitSkills.FARMING.name;
        startCondition = () => {
            if (!performer.take<UnitEquipmentComponent>().toolWithActionEquipped(TOOL_ACTION_NAME)) return tryCreateEquippingAction();
            if (!zone.take<ZoneComponent>().tiles.Contains(tile)) return FAIL;
            if (ZoneUtils.tileHoed(tile, model)) {
                log("tile already hoed");
                return FAIL;
            }
            lockZoneTile(zone, tile);
            return OK;
        };

        onStart = () => {
            maxProgress = 100;
            speed = getSpeed();
        };
        
        onFinish = () => {
            hoeTile(tile);
            unlockZoneTile(zone, tile);
            giveExp(5);
        };
    }

    private ActionCheckingEnum tryCreateEquippingAction() {
        ItemSelector toolItemSelector = new ToolWithActionItemSelector(TOOL_ACTION_NAME);
        EcsEntity item = model.itemContainer.findingUtil.findNearestItemBySelector(toolItemSelector, performer.pos());
        if (item == EcsEntity.Null) return FAIL;
        addPreAction(new EquipToolItemAction(item));
        return NEW;
    }

    private void hoeTile(Vector3Int tile) {
        model.farmContainer.addFarm(tile); // triggers tile update 
        model.plantContainer.removePlant(tile); // triggers tile update
        model.localMap.substrateMap.remove(tile);
    }
    
    // validates that tile is not hoed already
    public override bool validate() {
        return base.validate()
               && zone.IsAlive()
               && zone.take<ZoneComponent>().tiles.Contains(target.pos)
               && !model.farmContainer.isFarm(target.pos);
    }
}
}