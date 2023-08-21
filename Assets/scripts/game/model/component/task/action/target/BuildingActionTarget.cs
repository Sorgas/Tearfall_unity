using System.Collections.Generic;
using game.model.component.building;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
// points to tiles adjacent to building
public class BuildingActionTarget : EntityActionTarget {
    private List<Vector3Int> adjacentPositions = new(); // not filtered by tile passage
    
    public BuildingActionTarget(EcsEntity entity, ActionTargetTypeEnum placement) : base(entity, placement) {
        calculateAdjacentPositions();
        multitile = entity.take<BuildingComponent>().type.getSizeByOrientation(Orientations.N) != Vector2Int.one;
    }
    
    public override List<Vector3Int> getPositions(LocalModel model) {
        return adjacentPositions;
    }

    private void calculateAdjacentPositions() {
        BuildingComponent building = entity.take<BuildingComponent>();
        Vector2Int size = building.type.getSizeByOrientation(building.orientation);
        Vector3Int position = entity.pos();
        for (int x = -1; x <= size.x; x++) {
            adjacentPositions.Add(new Vector3Int(x, -1, 0) + position);
            adjacentPositions.Add(new Vector3Int(x, size.y, 0) + position);
        }
        for (int y = 0; y < size.x; y++) {
            adjacentPositions.Add(new Vector3Int(-1, y, 0) + position);
            adjacentPositions.Add(new Vector3Int(size.x, y, 0) + position);
        }
    }
}
}