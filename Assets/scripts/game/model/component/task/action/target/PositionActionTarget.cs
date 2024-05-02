using System.Collections.Generic;
using System.Linq;
using game.model.localmap;
using types;
using types.action;
using UnityEngine;
using util.geometry;
using static types.action.ActionTargetTypeEnum;

namespace game.model.component.task.action.target {
public sealed class PositionActionTarget : StaticActionTarget {
    private Vector3Int position;
    private bool sameLevelOnly;

    public PositionActionTarget(Vector3Int targetPosition, ActionTargetTypeEnum placement, bool sameLevelOnly = false) : base(placement) {
        position = targetPosition;
        this.sameLevelOnly = sameLevelOnly;
        init();
    }

    protected override Vector3Int calculatePosition() {
        return position;
    }

    protected override List<Vector3Int> calculatePositions() {
        return PositionUtil.allNeighbour
            .Select(delta => position + delta)
            .ToList();
    }

    protected override List<Vector3Int> calculateLowerPositions() {
        if (sameLevelOnly) return emptyList;
        return PositionUtil.allNeighbour
            .Select(delta => position + delta + Vector3Int.back)
            .ToList();
    }

    protected override List<Vector3Int> calculateUpperPositions() {
        if (sameLevelOnly) return emptyList;
        return PositionUtil.allNeighbour
            .Select(delta => position + delta + Vector3Int.forward)
            .ToList();
    }

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        List<Vector3Int> result = new();
        if (targetType == EXACT || targetType == ANY) {
            result.Add(position);
        }
        if (targetType == NEAR || targetType == ANY) {
            LocalMap map = model.localMap;
            result.AddRange(positions
                .Where(pos => model.localMap.inMap(pos))
                .Where(pos => map.passageMap.getPassage(pos) == PassageTypes.PASSABLE.VALUE)
                .ToList());
            if (!sameLevelOnly) {
                Vector3Int upperPos = position + Vector3Int.forward;
                Vector3Int lowerPos = position + Vector3Int.back;
                byte currentBlockType = map.blockType.get(position);
                // stairs give availability to lower or upper stairs
                if (isStairsChain(position, upperPos, map)) result.Add(upperPos);
                if (isStairsChain(lowerPos, position, map)) result.Add(lowerPos);
                // ramp is available if not blocked by above tile
                if (currentBlockType == BlockTypes.RAMP.CODE && map.blockType.get(upperPos) == BlockTypes.SPACE.CODE) {
                    result.AddRange(upperPositions
                        .Where(pos => model.localMap.inMap(pos))
                        .Where(pos => map.passageMap.getPassage(pos) == PassageTypes.PASSABLE.VALUE)
                        .ToList());
                }
                // lower unblocked ramp give availability to tile
                result.AddRange(lowerPositions
                    .Where(pos => model.localMap.inMap(pos))
                    .Where(pos => map.blockType.get(pos) == BlockTypes.RAMP.CODE)
                    .Where(pos => map.blockType.get(pos.x, pos.y, pos.z + 1) == BlockTypes.SPACE.CODE)
                    .ToList());
            }
        }
        return result
            .Where(position => model.localMap.inMap(position))
            .Where(position => model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE)
            .ToList();
    }

    private bool isStairsChain(Vector3Int lower, Vector3Int upper, LocalMap map) {
        byte upperType = map.blockType.get(upper);
        return map.inMap(lower)
            && map.inMap(upper)
            && map.blockType.get(lower) == BlockTypes.STAIRS.CODE
            && upperType == BlockTypes.STAIRS.CODE || upperType == BlockTypes.DOWNSTAIRS.CODE;
    }
    }
}