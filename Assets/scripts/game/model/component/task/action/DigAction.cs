using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.localmap;
using game.model.util;
using generation.item;
using Leopotam.Ecs;
using types;
using types.action;
using types.unit.skill;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionCheckingEnum;
using static types.BlockTypes;
using Random = UnityEngine.Random;

namespace game.model.component.task.action {
// Action for digging a tile of map. Performer should have tool with dig action in hands
// Skill boosts speed up to 160%. Yield is not affected
public class DigAction : ToolAction {
    private readonly DesignationType dessignationType;

    public DigAction(Vector3Int position, DesignationType type) : base("dig", new DiggingActionTarget(position, type.getDiggingBlockType())) {
        name = "dig action";
        animation = "mining";
        usedSkill = UnitSkills.MINING.name;
        dessignationType = type;

        startCondition = () => {
            if (!dessignationType.validator.validate(target.pos, model)) return FAIL; // tile became invalid
            if (!performer.take<UnitEquipmentComponent>().toolWithActionEquipped(toolAction))
                return addEquipAction(); // find tool
            return OK; // tool already equipped
        };

        onStart = () => {
            maxProgress = new DiggingWorkAmountCalculator().getWorkAmount(position, type, model);
            speed = getSpeed();
        };

        onFinish = () => {
            if (!dessignationType.validator.validate(target.pos, model)) return;
            updateMap();
            // TODO give exp
        };
    }

    private ActionCheckingEnum addEquipAction() {
        EcsEntity tool = model.itemContainer.findingUtil.findNearestItemBySelector(toolSelector, performer.pos());
        if (tool != EcsEntity.Null) {
            lockEntity(tool);
            return addPreAction(new EquipToolItemAction(tool));
        }
        return FAIL;
    }

    // Applies changes to local map. Some types of digging change not only target tile.
    private void updateMap() {
        LocalMap map = model.localMap;
        Vector3Int targetPos = target.pos;
        switch (dessignationType.name) {
            case "dig":
                updateAndRevealMap(targetPos, FLOOR);
                break;
            case "stairs":
                // TODO fix type selection
                updateAndRevealMap(targetPos, map.blockType.get(targetPos) == WALL.CODE ? STAIRS : DOWNSTAIRS);
                break;
            case "ramp":
                updateAndRevealMap(targetPos, RAMP);
                updateAndRevealMap(targetPos + Vector3Int.forward, SPACE);
                break;
            case "channel":
                updateAndRevealMap(targetPos, SPACE); // current
                Vector3Int rampPosition = targetPos + Vector3Int.back;
                if (map.blockType.get(rampPosition) == WALL.CODE) updateAndRevealMap(rampPosition, RAMP); // lower
                break;
            case "downstairs":
                byte currentType = map.blockType.get(targetPos);
                if (currentType != STAIRS.CODE && currentType != DOWNSTAIRS.CODE) updateAndRevealMap(targetPos, DOWNSTAIRS); // current 
                Vector3Int stairsPosition = targetPos + Vector3Int.back;
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
        if (Random.Range(0, 1f) > (newType.OPENNESS - oldType.OPENNESS) / 16f) return;
        EcsEntity item = new DiggingProductGenerator().generate(model.localMap.blockType.getMaterial(position), model);
        if (item != EcsEntity.Null) model.itemContainer.onMap.putItemToMap(item, position);
    }
}
}