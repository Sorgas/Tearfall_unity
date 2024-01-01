using System.Collections.Generic;
using UnityEngine;
using static types.PassageTypes;

namespace game.model.localmap.passage {
// helper for splitting map into rooms. When areas get split or merged, updates rooms in RoomContainer
public class RoomPassageHelper : AbstractPassageHelper {
    public RoomPassageHelper(PassageMap passage, LocalModel model) : base(passage, model) {
        updater.mergingCallback = vectors => model.roomContainer.roomsMerged(vectors);
        updater.splittingCallback = vectors => model.roomContainer.roomsSplit(vectors);
    }
    
    public override bool tileCanHaveArea(int x, int y, int z) {
        return passage.getPassage(x, y, z) == PASSABLE.VALUE;
    }

    public override bool hasPathBetweenNeighbours(int x1, int y1, int z1, int x2, int y2, int z2) {
        if (!map.inMap(x1, y1, z1) || !map.inMap(x2, y2, z2)) return false;
        int passageValue = passage.getPassage(x1, y1, z1);
        if(passageValue != PASSABLE.VALUE) return false;
        passageValue = passage.getPassage(x2, y2, z2); 
        if(passageValue != PASSABLE.VALUE) return false;
        return z1 == z2; // passable tiles on same level
    }

    public List<Vector3Int> floodFill(Vector3Int startPosition) {
        return null;
    }
}
}