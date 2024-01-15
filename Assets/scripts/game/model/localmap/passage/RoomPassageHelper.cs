using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using util.geometry;
using util.lang.extension;
using static types.PassageTypes;

namespace game.model.localmap.passage {
// helper for splitting map into rooms. When areas get split or merged, updates rooms in RoomContainer
public class RoomPassageHelper : AbstractPassageHelper {
    public RoomPassageHelper(PassageMap passage, LocalModel model) : base(passage, model) {
        updater.mergingCallback = vectors => model.roomContainer.roomsMerged(vectors);
        updater.splittingCallback = vectors => model.roomContainer.roomsSplit(vectors);
    }
    
    public override bool tileCanHaveArea(int x, int y, int z) => isPassable(passage.getPassage(x, y, z));

    public override bool hasPathBetweenNeighbours(int x1, int y1, int z1, int x2, int y2, int z2) {
        if (z1 != z2) return false;
        if (!map.inMap(x1, y1, z1) || !map.inMap(x2, y2, z2)) return false;
        if(!isPassable(passage.getPassage(x1, y1, z1)) || !isPassable(passage.getPassage(x2, y2, z2))) return false;
        return z1 == z2; // passable tiles on same level
    }

    public HashSet<Vector3Int> floodFill(Vector3Int startPosition) {
        ushort startingArea = area.get(startPosition);
        int size = area.sizes[startingArea];
        if (size > 400) {
            Debug.Log("area too big for room");
            return null;
        }
        List<Vector3Int> open = new();
        HashSet<Vector3Int> closed = new();
        open.Add(startPosition);
        while (open.Count > 0) {
            Vector3Int current = open.removeAndGet(0);
            PositionUtil.allNeighbour
                .Select(offset => current + offset)
                .Where(pos => area.get(pos) == startingArea)
                .Where(pos => !closed.Contains(pos))
                .ForEach(pos => open.Add(pos));
            closed.Add(current);
        }
        return closed;
    }

    // see passageValue in PassageTypes
    private bool isPassable(byte passageValue) {
        return passageValue == PASSABLE.VALUE || passageValue == IMPASSABLE_BUILDING.VALUE;
    }
}
}