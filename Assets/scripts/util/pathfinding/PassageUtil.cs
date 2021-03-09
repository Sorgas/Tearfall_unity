using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool positionReachable(IntVector3 from, IntVector3 to, bool acceptNearTarget) {
            if (from == null || to == null) return false;
            byte fromArea = passage.area.get(from);
            if (passage.area.get(to) == fromArea) return true; // target in same area

            return acceptNearTarget && PositionUtil.allNeighbour
                    .Select(pos => IntVector3.add(to, pos))
                    .Where(pos => map.inMap(pos))
                    .Select(pos => passage.area.get(pos))
                    .Any(area => area == fromArea);
        }
    }
}
