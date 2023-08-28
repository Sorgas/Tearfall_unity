using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.item;
using util.lang;
using util.lang.extension;
using static types.ZoneTaskTypes;

namespace game.model.action.plant.farm {
// common action created by farms, for doing all operations on farm tiles.
// gets nearest tile that requires action and creates sub-action for it
public class FarmingAction : ToolAction {
    private EcsEntity zone;

    public FarmingAction(EcsEntity zone) : base("hoe", new ZoneActionTarget(zone, ActionTargetTypeEnum.ANY)) {
        name = "work on farm " + zone.name();
        this.zone = zone;
    
        startCondition = () => {
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            Vector3Int performerPosition = performer.pos();
            List<Vector3Int> tiles = tracking.tilesToTask.Keys
                .OrderBy(tile => tile == performerPosition ? 3 : (tile - performerPosition).sqrMagnitude)
                .ToList();
            CachedVariable<bool> hoeAvailability = createHoeAvailability();
            foreach (Vector3Int tile in tiles) {
                if (!tileCanBeLocked(zone, tile)) continue; // tile is locked by another task
                string taskType = tracking.tilesToTask[tile];
                log(taskType + tile);
                if (taskType == HOE) {
                    if (hoeAvailability.value) return addPreAction(new FarmTileHoeingAction(tile, zone));
                } else if (taskType == PLANT) {
                    return addPreAction(new PlantSeedToTileAction(tile, zone)); // TODO seeds? use CachedVariable
                } else if (taskType == HARVEST) {
                    return addPreAction(new PlantHarvestAction(model.plantContainer.getPlant(tile)));
                } else if (taskType == REMOVE_PLANT) {
                    return addPreAction(new PlantRemoveAction(model.plantContainer.getPlant(tile)));
                }
                log("invalid type");
            }
            return ActionCheckingEnum.OK; // no more tiles to work on
        };
    }

    private CachedVariable<bool> createHoeAvailability() {
        return new CachedVariable<bool>(() => {
            if (performer.take<UnitEquipmentComponent>().toolWithActionEquipped(toolAction)) return true;
            ItemSelector toolItemSelector = new ToolWithActionItemSelector(toolAction);
            EcsEntity item = model.itemContainer.findingUtil.findNearestItemBySelector(toolItemSelector, performer.pos());
            return item != EcsEntity.Null;
        });
    }
}
}