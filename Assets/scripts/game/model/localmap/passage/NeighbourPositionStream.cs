using System.Collections.Generic;
using System.Linq;
using types;
using UnityEngine;

namespace game.model.localmap.passage {
    //TODO refactor constructors to chains
    class NeighbourPositionStream {
        public IEnumerable<Vector3Int> stream;
        private Vector3Int center;
        private LocalMap localMap;
        private PassageMap passageMap;

        public NeighbourPositionStream(Vector3Int center, LocalMap map) : this(map) {
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
        
        public NeighbourPositionStream(LocalMap map) {
            localMap = map;
            passageMap = localMap.passageMap;
        }

        // Considers center tile to have given type. Used for checking during building and digging.
        public NeighbourPositionStream filterConnectedToCenterWithOverrideTile(BlockType type) {
            stream = stream.Where(position => localMap.passageUtil.hasPathBetweenNeighboursWithOverride(center, position, type));
            return this;
        }

        public NeighbourPositionStream filterByPassage(Passage passage) {
            stream = stream.Where(position => passageMap.passage.get(position) == passage.VALUE);
            return this;
        }
    }
}