using System;
using System.Collections.Generic;
using System.Linq;
using enums;
using UnityEngine;
using util.geometry;

namespace game.model.localmap.passage {
    class NeighbourPositionStream {
        public IEnumerable<Vector3Int> stream;
        private Vector3Int center;
        private PassageMap passageMap;
        private LocalMap localMap;

        public NeighbourPositionStream(Vector3Int center) : this() {
            this.center = center;
            HashSet<Vector3Int> neighbours = new HashSet<Vector3Int>();
            for (int x = center.x - 1; x < center.x + 2; x++) {
                for (int y = center.y - 1; y < center.y + 2; y++) {
                    for (int z = center.z - 1; z < center.z + 2; z++) {
                        Vector3Int position = new Vector3Int(x, y, z);
                        if (position != center) neighbours.Add(position);
                    }
                }
            }
            stream = neighbours.Where(position => localMap.inMap(position));
        }

        public NeighbourPositionStream(Vector3Int center, bool orthogonal) : this() {
            HashSet<Vector3Int> neighbours = new HashSet<Vector3Int>();


            neighbours.Add(center.add(1, 0, 0));
            neighbours.Add(center.add(-1, 0, 0));
            neighbours.Add(center.add(0, 1, 0));
            neighbours.Add(center.add(0, -1, 0));

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

        public NeighbourPositionStream filter(Func<Vector3Int, bool> predicate) {
            stream = stream.Where(predicate);
            return this;
        }
    }
}
