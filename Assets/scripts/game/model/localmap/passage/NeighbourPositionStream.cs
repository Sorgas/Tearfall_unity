using System;
using System.Collections.Generic;
using System.Linq;
using enums;
using types;
using UnityEngine;
using util.geometry;
using util.lang.extension;

namespace game.model.localmap.passage {
    //TODO refactor constructors to chains
    class NeighbourPositionStream {
        public IEnumerable<Vector3Int> stream;
        private Vector3Int center;
        private PassageMap passageMap;
        private LocalMap localMap;

        public NeighbourPositionStream(Vector3Int center) : this() {
            this.center = center;
            HashSet<Vector3Int> neighbours = new();
            for (int x = center.x - 1; x < center.x + 2; x++) {
                for (int y = center.y - 1; y < center.y + 2; y++) {
                    for (int z = center.z - 1; z < center.z + 2; z++) {
                        Vector3Int position = new(x, y, z);
                        if (position != center) neighbours.Add(position);
                    }
                }
            }
            stream = neighbours.Where(position => localMap.inMap(position));
        }

        public NeighbourPositionStream(Vector3Int center, bool orthogonal) : this() {
            HashSet<Vector3Int> neighbours = new();
            neighbours.Add(center.add(1, 0, 0));
            neighbours.Add(center.add(-1, 0, 0));
            neighbours.Add(center.add(0, 1, 0));
            neighbours.Add(center.add(0, -1, 0));
            stream = neighbours.Where(position => localMap.inMap(position));
        }

        public NeighbourPositionStream() {
            localMap = GameModel.localMap;
            passageMap = localMap.passageMap;
        }

        public NeighbourPositionStream onSameZLevel(Vector3Int center) {
            this.center = center;
            HashSet<Vector3Int> neighbours = new();
            for (int x = center.x - 1; x < center.x + 2; x++) {
                for (int y = center.y - 1; y < center.y + 2; y++) {
                        Vector3Int position = new(x, y, center.z);
                        if (position != center && localMap.inMap(position)) neighbours.Add(position);
                }
            }
            stream = neighbours.AsEnumerable();
            return this;
        }
        
        // clears all if center in not passable
        public NeighbourPositionStream filterConnectedToCenter() {
            stream = stream.Where(position => passageMap.hasPathBetweenNeighbours(position, center));
            return this;
        }

        // Considers center tile to have given type. Used for checking during building.
        public NeighbourPositionStream filterConnectedToCenterWithOverrideTile(BlockType type) {
            stream = stream.Where(position => passageMap.tileIsAccessibleFromNeighbour(center, position, type));
            return this;
        }

        public NeighbourPositionStream filterNotInArea(int value) {
            stream = stream.Where(position => passageMap.area.get(position) != value);
            return this;
        }

        public NeighbourPositionStream filterInArea(int value) {
            stream = stream.Where(position => passageMap.area.get(position) == value);
            return this;
        }

        public NeighbourPositionStream filterByPassage(Passage passage) {
            stream = stream.Where(position => passageMap.passage.get(position) == passage.VALUE);
            return this;
        }

        public NeighbourPositionStream filterSameZLevel() {
            stream = stream.Where(position => position.z == center.z);
            return this;
        }

        public NeighbourPositionStream filter(Func<Vector3Int, bool> predicate) {
            stream = stream.Where(predicate);
            return this;
        }

        public Dictionary<byte, List<Vector3Int>> groupByAreas() {
            return stream.ToDictionary(position => passageMap.area.get(position), position => new List<Vector3Int> { position }, (l, r) => { l.AddRange(r); return l; });
        }

        // collects areas of positions in stream
        public List<byte> collectAreas() {
            return stream.Select(position => passageMap.area.get(position)).Distinct().ToList();
        }
    }
}