using System.Collections.Generic;
using System.Linq;
using game.model.component.task.order;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry;
using util.geometry.bounds;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;

namespace game.model.component.task.action.target {
// targets designation. Allows interaction from adjacent tiles on same level. NEAR only
public class DesignationActionTarget : StaticActionTarget {
    private Vector3Int position;
    private IntBounds3 bounds;

    public DesignationActionTarget(EcsEntity designation) : base(NEAR) {
        position = designation.pos();
        if (designation.Has<DesignationBuildingComponent>()) { // create bounds if multi-tile designation
            BuildingOrder order = designation.take<DesignationBuildingComponent>().order;
            Vector3Int size = order.type.getSize3ByOrientation(order.orientation);
            if (size.x > 1 || size.y > 1) bounds = IntBounds3.byStartAndSize(position, size);
        }
        init();
    }

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        return positions
            .Where(vector => model.localMap.inMap(vector))
            .Where(vector => model.localMap.passageMap.getPassage(vector) == PassageTypes.PASSABLE.VALUE)
            .ToList();
    }

    protected override Vector3Int calculatePosition() => position;

    protected override List<Vector3Int> calculatePositions() {
        if (bounds != null)
            return bounds.getExternalBorders(true)
                .Select(value => {
                    value.z = position.z;
                    return value;
                })
                .ToList();
        else
            return PositionUtil.allNeighbour
                .Select(delta => position + delta)
                .ToList();
    }
}
}