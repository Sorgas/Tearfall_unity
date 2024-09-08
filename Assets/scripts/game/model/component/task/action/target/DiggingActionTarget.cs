using System.Collections.Generic;
using System.Linq;
using game.model.localmap;
using game.model.localmap.passage;
using types;
using types.action;
using UnityEngine;
using util.geometry;

namespace game.model.component.task.action.target {
// targets tile to dig
public class DiggingActionTarget : StaticActionTarget {
    private Vector3Int center;
    private BlockType newBlockType;

    public DiggingActionTarget(Vector3Int position, BlockType targetBlockType) : base(ActionTargetTypeEnum.NEAR) {
        center = position;
        newBlockType = targetBlockType;
        init();
    }

    protected override Vector3Int calculateStaticPosition() {
        return center;
    }

    protected override List<Vector3Int> calculateStaticPositions() {
        return PositionUtil.allNeighbour
            .Select(delta => delta + center)
            .ToList();
    }

    // digging has special conditions of tile accessibility
    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        // return positions
        //     .Where(position => model.localMap.passageMap.hasPathBetweenNeighboursWithOverride(center, position, newBlockType))
        //     .ToList();
        return new NeighbourPositionStream(center, model.localMap)
            .filterConnectedToCenterWithOverrideTile(newBlockType).stream.ToList();
    }
}
}