using System.Linq;
using Assets.scripts.game.model.localmap;
using Assets.scripts.game.model.localmap.passage;
using Assets.scripts.util.geometry;
using UnityEngine;

namespace Assets.scripts.util.pathfinding {
    public class PassageUtil {
        private PassageMap passage;
        private LocalMap map;

        public PassageUtil(LocalMap map, PassageMap passage) {
            this.map = map;
            this.passage = passage;
        }

        public bool positionReachable(Vector3Int from, Vector3Int to, bool acceptNearTarget) {
            if (from == null || to == null) return false;
            byte fromArea = passage.area.get(from);
            if (passage.area.get(to) == fromArea) return true; // target in same area

            return acceptNearTarget && PositionUtil.allNeighbour
                    .Select(pos => to + pos)
                    .Where(pos => map.inMap(pos))
                    .Select(pos => passage.area.get(pos))
                    .Any(area => area == fromArea);
        }
    }
}
