using System.Collections.Generic;
using System.Linq;
using game.model.localmap;
using types;
using types.action;
using UnityEngine;
using util.geometry;

namespace game.model.component.task.action.target {
// finds unhoed tile 
public class FarmHoeingActionTarget : StaticActionTarget {
    private Vector3Int tile;

    public FarmHoeingActionTarget(Vector3Int tile) : base(ActionTargetTypeEnum.NEAR) {
        this.tile = tile;
    }
    
    protected override Vector3Int calculatePosition() => tile;

    protected override List<Vector3Int> calculatePositions() {
        return PositionUtil.allNeighbour
            .Select(delta => tile + delta)
            .ToList();
    }

    // public Vector3Int lookupFreeTile() {
    //     position = ZoneUtils.findUnhoedTile(zone, GameModel.get().currentLocalModel);
    //     return position;
    // }
    //
    // public Vector3Int lookupFreeNearestTile(Vector3Int pos) {
    //     position = ZoneUtils.findNearestUnhoedTile(zone, pos, GameModel.get().currentLocalModel);
    //     return position;
    // }
    
    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        return positions
            .Where(position => model.localMap.inMap(position))
            .Where(position => model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE)
            .ToList();
    }
}
}