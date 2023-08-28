using System.Collections.Generic;
using System.Linq;
using game.model.localmap;
using types.action;
using UnityEngine;
using util.geometry;

namespace game.model.component.task.action.target {
public sealed class PositionActionTarget : StaticActionTarget {
    private Vector3Int position;

    public PositionActionTarget(Vector3Int targetPosition, ActionTargetTypeEnum placement) : base(placement) {
        position = targetPosition;
        init();
    }

    protected override Vector3Int calculatePosition() {
        return position;
    }

    protected override List<Vector3Int> calculatePositions() {
        return PositionUtil.allNeighbour
            .Select(delta => position + delta)
            .SelectMany(vector => Enumerable.Repeat(vector, 2))
            .Select((vector, i) => i % 2 == 0 ? vector.add(0, 0, -1) : vector)
            .ToList();
    }

    protected override List<Vector3Int> calculateLowerPositions() {
        return PositionUtil.allNeighbourWithCenter
            .Select(delta => position + delta)
            .SelectMany(vector => Enumerable.Repeat(vector, 2))
            .Select(vector => vector.add(0, 0, -1))
            .ToList();
    }

    protected override List<Vector3Int> calculateUpperPositions() {
        return PositionUtil.allNeighbourWithCenter
            .Select(delta => position + delta)
            .Select(vector => vector.add(0, 0, 1))
            .ToList();
    }

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        throw new System.NotImplementedException();
    }
}
}