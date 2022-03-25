using System;
using enums;
using enums.action;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.item;
using util.lang.extension;
using static enums.action.ActionConditionStatusEnum;
using static enums.action.ActionTargetTypeEnum;

namespace game.model.component.task.action {
    public class DigAction : Action {
        private DesignationType type;
        private float workAmountModifier = 10f;
        private String toolActionName = "dig";
        private ToolWithActionItemSelector selector;

        public DigAction(Vector3Int position, DesignationType type) : base(new PositionActionTarget(position, NEAR), "mining") {
            name = "dig action";
            this.type = type;
            selector = new ToolWithActionItemSelector(toolActionName);
            startCondition = () => {
                if (!type.VALIDATOR.validate(target.getPos().Value)) return FAIL; // tile still valid
                if (!performer().Has<UnitEquipmentComponent>()) return FAIL;
                if (performer().Get<UnitEquipmentComponent>().toolWithActionEquipped(toolActionName))
                    return OK; // tool already equipped
                return addEquipAction();
            };

            onStart = () => { // TODO
                // speed = 1 + skill().speed * performerLevel() + performance();
                // maxProgress = getWorkAmount(designation) * workAmountModifier; // 480 for wall to floor in marble
            };

            onFinish = () => {
                Vector3Int targetPosition = target.getPos().Value;
                // if (targetPosition.HasValue != null) {
                BlockType blockType = GameModel.localMap.blockType.getEnumValue(targetPosition);
                // }
                if (!type.VALIDATOR.validate(targetPosition)) return;
                updateMap();
                // leaveStone(oldType); TODO
                // GameMvc.model().get(UnitContainer.class).experienceSystem.giveExperience(task.performer, skill);
                // GameMvc.model().get(TaskContainer.class).designationSystem.removeDesignation(designation.position);
            };
        }

        private ActionConditionStatusEnum addEquipAction() {
            // TODO check performer's 'backpack'
            EcsEntity? targetItem = GameModel.get().itemContainer.util
                .findFreeReachableItemBySelector(selector, performer().pos());
            return targetItem.HasValue
                ? addPreAction(new EquipToolItemAction(targetItem.Value))
                : FAIL;
        }

        // Applies changes to local map. Some types of digging change not only target tile.
        private void updateMap() {
            //        LocalMap map = GameMvc.model().get(LocalMap.class);
            //        IntVector3 target = this.target.getPosition();
            //        switch (type) {
            //            case D_DIG:
            //                updateAndRevealMap(target, FLOOR);
            //                break;
            //            case D_STAIRS:
            //                if (map.blockType.get(target) == WALL.CODE) {
            //                    updateAndRevealMap(target, STAIRS);
            //                } else {
            //                    updateAndRevealMap(target, DOWNSTAIRS);
            //                }
            //                break;
            //            case D_RAMP:
            //                updateAndRevealMap(target, RAMP);
            //                IntVector3 upperPosition = new IntVector3(target.x, target.y, target.z + 1);
            //                if (map.inMap(upperPosition))
            //                    updateAndRevealMap(upperPosition, SPACE);
            //                break;
            //            case D_CHANNEL:
            //                updateAndRevealMap(target, SPACE);
            //                IntVector3 rampPosition = new IntVector3(target.x, target.y, target.z - 1);
            //                if (map.inMap(rampPosition) && map.blockType.get(rampPosition) == WALL.CODE)
            //                    updateAndRevealMap(rampPosition, RAMP);
            //                break;
            //            case D_DOWNSTAIRS:
            //                updateAndRevealMap(target, DOWNSTAIRS);
            //                IntVector3 stairsPosition = new IntVector3(target.x, target.y, target.z - 1);
            //                if (map.inMap(stairsPosition) && map.blockType.get(stairsPosition) == WALL.CODE)
            //                    updateAndRevealMap(stairsPosition, STAIRS);
            //                break;
            //        }
            //    }
            //
            //    /**
            // * Puts rock of dug material if needed.
            // */
            //    private void leaveStone(BlockTypeEnum oldType) {
            //        LocalMap map = GameMvc.model().get(LocalMap.class);
            //        ItemContainer container = GameMvc.model().get(ItemContainer.class);
            //        IntVector3 target = this.target.getPosition();
            //        BlockTypeEnum newType = map.blockType.getEnumValue(target);
            //        int materialId = map.blockType.getMaterial(target);
            //        new DiggingProductGenerator()
            //            .generateDigProduct(materialId, oldType, newType)
            //            .forEach(item->container.onMapItemsSystem.addNewItemToMap(item, target));
            //    }
            //
            //    private void updateAndRevealMap(IntVector3 position, BlockTypeEnum type) {
            //        LocalMap map = GameMvc.model().get(LocalMap.class);
            //        map.blockType.set(position, type);
            //        map.light.handleDigging(position);
            //    }
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
            //     Debug.Log("Dig action created for " + position.position);
            //
        }
    }
}