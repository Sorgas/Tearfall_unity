using System.Linq;
using game.model.localmap;
using game.model.localmap.passage;
using UnityEngine;
using util.geometry;

namespace util.pathfinding {
    // TODO delete file
    // public class PassageUtil {
    //     private readonly PassageMap passage;
    //     private readonly LocalMap map;
    //
    //     public PassageUtil(LocalMap map, PassageMap passage) {
    //         this.map = map;
    //         this.passage = passage;
    //     }
    //
    //     public bool positionReachable(Vector3Int? from, Vector3Int? to, bool acceptNearTarget) {
    //         if (from == null || to == null) return false;
    //         var fromArea = passage.area.get(from.Value);
    //         if (passage.area.get(to.Value) == fromArea) return true; // target in same area
    //
    //         return acceptNearTarget && PositionUtil.allNeighbour
    //             .Select(pos => to + pos)
    //             .Where(pos => map.inMap(pos.Value))
    //             .Select(pos => passage.area.get(pos.Value))
    //             .Any(area => area == fromArea);
    //     }
    // }
}