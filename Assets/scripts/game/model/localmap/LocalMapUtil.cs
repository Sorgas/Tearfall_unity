using System;
using System.Collections.Generic;
using System.Linq;
using game.model.localmap.passage;
using types;
using UnityEngine;
using util.geometry.bounds;
using Random = UnityEngine.Random;

namespace game.model.localmap {
    public class LocalMapUtil {
        private LocalModel model;
        private LocalMap map;

        public LocalMapUtil(LocalMap map) {
            this.map = map;
        }

        public Vector3Int? getRandomPosition(Vector3Int center, int xRange, int zRange) {
            IntBounds3 bounds = new (center, xRange, xRange, zRange);
            map.bounds.normalizeBounds(bounds);
            List<Vector3Int> positions = new List<Vector3Int>();
            bounds.iterate((x, y, z) => {
                int blockType = map.blockType.get(x, y, z);
                if (blockType != BlockTypes.SPACE.CODE
                    && blockType != BlockTypes.WALL.CODE) { // passable position
                    positions.Add(new Vector3Int(x, y, z));
                }
            });
            return positions.Count != 0 ? positions[Random.Range(0, positions.Count - 1)] : (Vector3Int?)null;
        }

        // change postition to move it inside map
        // TODO move to IntBounds3
        public void normalizePosition(Vector3Int position) => normalizeRectangle(ref position, 1, 1);

        // change position to move rectangle with position in [0,0] inside map
        // TODO move to IntBounds3
        public void normalizeRectangle(ref Vector3Int position, int width, int height) {
            position.x = Math.Min(Math.Max(0, position.x), map.bounds.maxX - width);
            position.y = Math.Min(Math.Max(0, position.y), map.bounds.maxY - height);
            position.z = Math.Min(Math.Max(0, position.z), map.bounds.maxZ - 1);
        }

        public Vector3Int findFreePositionNearCenter(Vector3Int center) {
            Vector3Int position = new NeighbourPositionStream(center, map).filterByPassage(PassageTypes.PASSABLE).stream.First();
            return position;
        }
    }
}