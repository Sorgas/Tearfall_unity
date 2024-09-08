using System.Collections.Generic;
using System.Linq;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.geometry;
using util.lang.extension;

namespace game.model.component.task.action.target {
// targets plant for harvesting, cutting or chopping
public class PlantActionTarget : StaticActionTarget {
    private readonly EcsEntity plant;

    public PlantActionTarget(EcsEntity plant, ActionTargetTypeEnum type = ActionTargetTypeEnum.ANY) : base(type) {
        this.plant = plant;
        init();
    }

    protected override Vector3Int calculateStaticPosition() {
        return plant.pos();
    }

    protected override List<Vector3Int> calculateStaticPositions() {
        return PositionUtil.allNeighbour
            .Select(delta => pos + delta)
            .ToList();
    }

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        return PositionUtil.allNeighbourWithCenter
            .Select(delta => pos + delta)
            .Where(position => model.localMap.inMap(position))
            .Where(position => model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE)
            .ToList();
    }
}
}