using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.enums;
using Assets.scripts.util.geometry;

namespace Assets.scripts.game.model.localmap.passage {
    class NeighbourPositionStream {
        public IEnumerable<IntVector3> stream;
        private IntVector3 center;
        private PassageMap passageMap;
        private LocalMap localMap;

        public NeighbourPositionStream(IntVector3 center) : this() {
            this.center = center;
            HashSet<IntVector3> neighbours = new HashSet<IntVector3>();
            for (int x = center.x - 1; x < center.x + 2; x++) {
                for (int y = center.y - 1; y < center.y + 2; y++) {
                    for (int z = center.z - 1; z < center.z + 2; z++) {
                        IntVector3 position = new IntVector3(x, y, z);
                        if (position != center) neighbours.Add(position);
                    }
                }
            }
            stream = neighbours.Where(position => localMap.inMap(position));
        }

        public NeighbourPositionStream(IntVector3 center, bool orthogonal) : this() {
            HashSet<IntVector3> neighbours = new HashSet<IntVector3>();


            neighbours.Add(IntVector3.add(center, 1, 0, 0));
            neighbours.Add(IntVector3.add(center, -1, 0, 0));
            neighbours.Add(IntVector3.add(center, 0, 1, 0));
            neighbours.Add(IntVector3.add(center, 0, -1, 0));

            stream = neighbours.Where(position => localMap.inMap(position));
        }

        private NeighbourPositionStream() {
            localMap = GameModel.get().localMap;
            passageMap = localMap.passageMap;
        }

        /**
         * Filters all tiles where walking creature cannot step into.
         * Clears all, if center tile is not passable.
         */
        public NeighbourPositionStream filterConnectedToCenter() {
            stream = stream.Where(position=>passageMap.hasPathBetweenNeighbours(position, center));
            return this;
        }

        /**
         * Considers center tile to have given type. Used for checking during building.
         */
        public NeighbourPositionStream filterByAccessibilityWithFutureTile(BlockType type) {
            stream = stream.Where(position=>passageMap.tileIsAccessibleFromNeighbour(center, position, type));
            return this;
        }

        public NeighbourPositionStream filterNotInArea(int value) {
            stream = stream.Where(position=>passageMap.area.get(position) != value);
            return this;
        }

        public NeighbourPositionStream filterInArea(int value) {
            stream = stream.Where(position=>passageMap.area.get(position) == value);
            return this;
        }

        public NeighbourPositionStream filterByPassage(Passage passage) {
            stream = stream.Where(position=>passageMap.passage.get(position) == passage.VALUE);
            return this;
        }

        public NeighbourPositionStream filterSameZLevel() {
            stream = stream.Where(position=>position.z == center.z);
            return this;
        }

        public NeighbourPositionStream filter(Func<IntVector3, bool> predicate) {
            stream = stream.Where(predicate);
            return this;
        }
    }
}
