using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.task.action;
using game.model.component.task.action.plant;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.item;
using util.lang;
using util.lang.extension;
using static types.ZoneTaskTypes;

namespace game.model.task.plant {
    // common action for doing all operations on farm tiles.
    // gets nearest tile that requires action and creates sub-action for it
    public class FarmingAction : Action {
        private const string TOOL_ACTION = "hoe";
        private EcsEntity zone;
        public FarmingAction(EcsEntity zone) : base(new ZoneActionTarget(zone, ActionTargetTypeEnum.ANY)) {
            this.zone = zone;
            name = "work on farm " + zone.name();
            startCondition = () => {
                ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
                Vector3Int performerPosition = performer.pos();
                List<Vector3Int> tiles = tracking.tilesToTask.Keys
                    .OrderBy(tile => (tile - performerPosition).sqrMagnitude)
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
                return ActionConditionStatusEnum.OK; // no more tiles to work on
            };
        }

        private CachedVariable<bool> createHoeAvailability() {
            return new CachedVariable<bool>(() => {
                if (performer.take<UnitEquipmentComponent>().toolWithActionEquipped(TOOL_ACTION)) return true;
                ItemSelector toolItemSelector = new ToolWithActionItemSelector(TOOL_ACTION);
                EcsEntity item = model.itemContainer.util.findFreeReachableItemBySelector(toolItemSelector, performer.pos());
                return item != EcsEntity.Null;
            });
        }
    }
}