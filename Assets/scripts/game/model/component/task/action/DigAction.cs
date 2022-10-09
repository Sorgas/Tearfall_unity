using System;
using enums.action;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.localmap;
using generation.item;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.item;
using util.lang.extension;
using static enums.action.ActionConditionStatusEnum;
using static types.BlockTypes;
using Random = UnityEngine.Random;

namespace game.model.component.task.action {
    // digs a tile of map. performer should have tool with dig action in hands 
    public class DigAction : Action {
        private float workAmountModifier = 10f;
        private String toolActionName = "dig";
        private ToolWithActionItemSelector selector;
        private DesignationType type;

        public DigAction(Vector3Int position, DesignationType type) : base(new PositionActionTarget(position, type.targetType)) {
            name = "dig action";
            selector = new ToolWithActionItemSelector(toolActionName);
            this.type = type;

            startCondition = () => {
                if (!type.validator.validate(target.getPos().Value, model)) return FAIL; // tile still valid
                if (!performer.Has<UnitEquipmentComponent>()) return FAIL;
                if (!performer.take<UnitEquipmentComponent>().toolWithActionEquipped(toolActionName)) 
                    return addEquipAction(); // find tool
                return OK; // tool already equipped
            };

            onStart = () => { // TODO
                maxProgress = 10;
                speed = 1;
                // speed = 1 + skill().speed * performerLevel() + performance();
                // maxProgress = getWorkAmount(designation) * workAmountModifier; // 480 for wall to floor in marble
            };

            onFinish = () => {
                if (!type.validator.validate(target.getPos().Value, model)) return;
                updateMap();
                // leaveStone(oldType); TODO
                // GameMvc.model().get(UnitContainer.class).experienceSystem.giveExperience(task.performer, skill);
                // GameMvc.model().get(TaskContainer.class).designationSystem.removeDesignation(designation.position);
            };
        }

        private ActionConditionStatusEnum addEquipAction() {
            // TODO check performer's 'backpack'
            EcsEntity targetItem = model.itemContainer.util
                .findFreeReachableItemBySelector(selector, performer.pos());
            return targetItem != EcsEntity.Null
                ? addPreAction(new EquipToolItemAction(targetItem))
                : FAIL;
        }

        // Applies changes to local map. Some types of digging change not only target tile.
        private void updateMap() {
            LocalMap map = model.localMap;
            Vector3Int target = this.target.getPos().Value;
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
        
        //
        //    /**
        // * Work amount is based on material hardness and volume of dug stone.
        // * TODO cover with test
        // */
        //    private float getWorkAmount(Designation designation) {
        //        LocalMap map = GameMvc.model().get(LocalMap.class);
        //        switch (designation.type) {
        //            case D_DIG:
        //                return getWorkAmountForTile(designation.position, map, FLOOR);
        //            case D_STAIRS:
        //                return getWorkAmountForTile(designation.position, map, STAIRS);
        //            case D_DOWNSTAIRS:
        //                return getWorkAmountForTile(designation.position, map, DOWNSTAIRS);
        //            case D_RAMP:
        //                IntVector3 upperPosition = IntVector3.add(designation.position, 0, 0, 1);
        //                return getWorkAmountForTile(designation.position, map, RAMP) + Math.max(getWorkAmountForTile(upperPosition, map, SPACE), 0);
        //            case D_CHANNEL:
        //                IntVector3 lowerPosition = IntVector3.add(designation.position, 0, 0, -1);
        //                return getWorkAmountForTile(designation.position, map, SPACE) + Math.max(getWorkAmountForTile(lowerPosition, map, RAMP), 0);
        //        }
        //        return Logger.TASKS.logError("Non-digging designation type in DigAction", 0);
        //    }

        // private float getWorkAmountForTile(IntVector3 position, LocalMap map, BlockTypeEnum targetType) {
        //     return MaterialMap.getMaterial(map.blockType.getMaterial(position)).density *
        //            (targetType.OPENNESS - map.blockType.getEnumValue(position).OPENNESS);
        // }
        //
        // public String toString() {
        //     return "Digging " + type;
        // }
        //
        //
        // public DigAction(DesignationComponent designation, PositionComponent position) : base(new PositionActionTarget(position.position, NEAR)) {
        //     log("Dig action created for " + position.position);
        //
        // }
    }
}