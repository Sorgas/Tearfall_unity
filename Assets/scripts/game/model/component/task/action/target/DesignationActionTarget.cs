using System.Collections.Generic;
using System.Linq;
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
            DesignationBuildingComponent buildingComponent = designation.take<DesignationBuildingComponent>();
            Vector3Int size = buildingComponent.type.getSize3ByOrientation(buildingComponent.orientation);
            if (size.x > 1 || size.y > 1) bounds = new(position, position + size);
        }
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