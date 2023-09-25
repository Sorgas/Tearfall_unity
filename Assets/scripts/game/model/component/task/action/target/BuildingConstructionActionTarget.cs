using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.task.order;
using game.model.localmap;
using MoreLinq;
using types;
using UnityEngine;
using util.geometry.bounds;
using static types.action.ActionTargetTypeEnum;

namespace game.model.component.task.action.target {
// action target for building buildings or constructions. Allows interacting from adjacent tiles on same z-level.
public class BuildingConstructionActionTarget : StaticActionTarget {
    public readonly Vector3Int center;
    private readonly IntBounds3 bounds;

    // public BuildingConstructionActionTarget(Vector3Int position) : base(NEAR) {
    //     Debug.Log("creating building construction action target for position");
    //     center = position;
    //     bounds = new IntBounds3(center, center);
    // }

    public BuildingConstructionActionTarget(GenericBuildingOrder order) : base(NEAR) {
        Debug.Log("creating building construction action target for order");
        center = order.position;
        if (order is BuildingOrder buildingOrder) {
            Vector2Int size = buildingOrder.type.getSizeByOrientation(buildingOrder.orientation);
            bounds = new IntBounds3(center.x, center.y, center.z, center.x + size.x - 1, center.y + size.y - 1, center.z);
        } else if (order is ConstructionOrder) {
            bounds = new IntBounds3(center, center);
        } else {
            throw new ArgumentException("Unsupported type of GenericBuildingOrder");
        }
    }

    protected override Vector3Int calculatePosition() => center;

    protected override List<Vector3Int> calculatePositions() => bounds.getExternalBorders(true)
        .Select(position => {
            position.z = center.z;
            return position;
        }).ToList();

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        List<Vector3Int> result = new();
        if (targetType == EXACT) {
            result = bounds.toList();
        } else if (targetType == NEAR) {
            positions
                .Where(position => model.localMap.inMap(position))
                .Where(position => model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE)
                .ForEach(position => result.Add(position));
            result.AddRange(positions);
        }
        return result;
    }
}
}