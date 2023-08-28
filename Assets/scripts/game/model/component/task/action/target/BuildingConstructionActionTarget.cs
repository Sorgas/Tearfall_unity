using System.Collections.Generic;
using System.Linq;
using game.model.component.task.order;
using game.model.localmap;
using types;
using UnityEngine;
using util.geometry.bounds;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;

namespace game.model.component.task.action.target {
// action target for building buildings. Allows interacting from adjacent tiles on same z-level.
public class BuildingConstructionActionTarget : StaticActionTarget {
    public readonly Vector3Int center;
    private readonly IntBounds3 bounds;

    public BuildingConstructionActionTarget(Vector3Int position) : base(NEAR) {
        center = position;
    }

    public BuildingConstructionActionTarget(ConstructionOrder order) : base(NEAR) {
        center = order.position;
        bounds = new IntBounds3(center, center);
    }

    public BuildingConstructionActionTarget(BuildingOrder order) : base(NEAR) {
        center = order.position;
        Vector2Int size = order.type.getSizeByOrientation(order.orientation);
        bounds = new IntBounds3(center.x, center.y, center.z, center.x + size.x - 1, center.y + size.y - 1, center.z);
    }

    protected override Vector3Int calculatePosition() => center;

    protected override List<Vector3Int> calculatePositions() => bounds.getExternalBorders(true);

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        List<Vector3Int> result = new();
        if (targetType == EXACT) {
            bounds.iterate(position => result.Add(position));
        }
        if (targetType == NEAR) {
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