using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.localmap;
using game.model.util;
using generation.item;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.item;
using util.lang.extension;
using static types.action.ActionConditionStatusEnum;
using static types.BlockTypes;
using Random = UnityEngine.Random;

namespace game.model.component.task.action {
    // digs a tile of map. performer should have tool with dig action in hands
    // TODO move looking for tool to dedicated action 
    public class DigAction : Action {
        private DesignationType type;
        private string toolActionName = "dig";
        private ToolWithActionItemSelector selector;

        public DigAction(Vector3Int position, DesignationType type) : base(new PositionActionTarget(position, type.targetType)) {
            name = "dig action";
            this.type = type;
            selector = new ToolWithActionItemSelector(toolActionName);

            startCondition = () => {
                if (!type.validator.validate(target.Pos.Value, model)) return FAIL; // tile became invalid
                if (!performer.Has<UnitEquipmentComponent>()) return FAIL;
                if (!performer.take<UnitEquipmentComponent>().toolWithActionEquipped(toolActionName)) 
                    return addEquipAction(); // find tool
                return OK; // tool already equipped
            };

            onStart = () => { // TODO consider performer skill
                maxProgress = new DiggingWorkAmountCalculator().getWorkAmount(position, type, model);
            };

            onFinish = () => {
                if (!type.validator.validate(target.Pos.Value, model)) return;
                updateMap();
                // TODO give exp
            };
        }

        private ActionConditionStatusEnum addEquipAction() {
            // TODO check performer's 'backpack'
            EcsEntity tool = model.itemContainer.util.findFreeReachableItemBySelector(selector, performer.pos());
            if(tool != EcsEntity.Null) {
                lockEntity(tool);
                return addPreAction(new EquipToolItemAction(tool));
            }
            return FAIL;
        }

        // Applies changes to local map. Some types of digging change not only target tile.
        private void updateMap() {
            LocalMap map = model.localMap;
            Vector3Int target = this.target.Pos.Value;
            switch (type.name) {
                case "dig":
                    updateAndRevealMap(target, FLOOR);
                    break;
                case "stairs":
                    // TODO fix type selection
                    updateAndRevealMap(target, map.blockType.get(target) == WALL.CODE ? STAIRS : DOWNSTAIRS);
                    break;
                case "ramp":
                    updateAndRevealMap(target, RAMP);
                    updateAndRevealMap(target + Vector3Int.forward, SPACE);
                    break;
                case "channel":
                    updateAndRevealMap(target, SPACE); // current
                    Vector3Int rampPosition = target + Vector3Int.back;
                    if (map.blockType.get(rampPosition) == WALL.CODE) updateAndRevealMap(rampPosition, RAMP); // lower
                    break;
                case "downstairs":
                    byte currentType = map.blockType.get(target);
                    if (currentType != STAIRS.CODE && currentType != DOWNSTAIRS.CODE) updateAndRevealMap(target, DOWNSTAIRS); // current 
                    Vector3Int stairsPosition = target + Vector3Int.back;
                    byte lowerType = map.blockType.get(stairsPosition);
                    if (lowerType == WALL.CODE || lowerType == RAMP.CODE) updateAndRevealMap(stairsPosition, STAIRS); // lower
                    break;
            }
        }

        private void updateAndRevealMap(Vector3Int position, BlockType type) {
            LocalMap map = model.localMap;
            if (map.inMap(position)) {
                BlockType oldType = map.blockType.getEnumValue(position);
                map.substrateMap.removeAndActivate(position);
                map.blockType.set(position, type);
                leaveStone(position, oldType, type);
                // map.light.handleDigging(position); TODO
            }
        }
        
        private void leaveStone(Vector3Int position, BlockType oldType, BlockType newType) {
            if(Random.Range(0, 1f) > (newType.OPENNESS - oldType.OPENNESS) / 16f) return;
            EcsEntity item = new DiggingProductGenerator().generate(model.localMap.blockType.getMaterial(position), model);
            if(item != EcsEntity.Null) model.itemContainer.onMap.putItemToMap(item, position);
        }      
    }
}