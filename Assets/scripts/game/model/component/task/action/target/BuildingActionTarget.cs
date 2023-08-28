using System.Collections.Generic;
using System.Linq;
using game.model.component.building;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
// targets building entity. Used for getting items from building.
// allows interacting with building from same and lower z-levels, but not from above.
public class BuildingActionTarget : StaticActionTarget {
    private EcsEntity building;
    public readonly List<Vector3Int> selfPositions = new();

    public BuildingActionTarget(EcsEntity entity, ActionTargetTypeEnum placement) : base(placement) {
        building = entity;
        init();
    }
    
    protected override Vector3Int calculatePosition() {
        return building.pos();
    }

    // positions around building on the same z-level 
    protected override List<Vector3Int> calculatePositions() {
        BuildingComponent component = building.take<BuildingComponent>();
        component.bounds.iterate(position => selfPositions.Add(position));
        return component.bounds.getExternalBorders(true);
    }

    // protected override List<Vector3Int> calculateLowerPositions() {
    //     return positions.Select(position => position + Vector3Int.back).ToList();
    // }

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        List<Vector3Int> result = new();
        positions
            .Where(position => model.localMap.inMap(position))
            .Where(position => model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE)
            .ForEach(position => result.Add(position));
        result.AddRange(positions);
        return result;
    }
}
}