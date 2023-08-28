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
    
    public DiggingActionTarget(ActionTargetTypeEnum type) : base(type) { }

    public DiggingActionTarget(Vector3Int position, BlockType targetBlockType) : base(ActionTargetTypeEnum.NEAR) {
        center = position;
        newBlockType = targetBlockType;
    }

    protected override Vector3Int calculatePosition() {
        return center;
    }

    protected override List<Vector3Int> calculatePositions() {
        return PositionUtil.allNeighbour
            .Select(delta => delta + center)
            .ToList();
    }

    protected override List<Vector3Int> calculateLowerPositions() {
        return positions
            .Select(position => position + Vector3Int.back)
            .ToList();
    }

    protected override List<Vector3Int> calculateUpperPositions() {
        return positions
            .Select(position => position + Vector3Int.forward)
            .ToList();
    }

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        return new NeighbourPositionStream(center, model)
            .filterConnectedToCenterWithOverrideTile(newBlockType).stream.ToList();
    }
}
}