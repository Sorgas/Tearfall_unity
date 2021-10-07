using System.Collections.Generic;
using enums;
using UnityEngine;
using util.geometry;

namespace game.model.localmap {
    public class LocalMapUtil {
        private LocalMap map;

        public LocalMapUtil(LocalMap map) {
            this.map = map;
        }

        public Vector3Int getRandomPosition(Vector3Int center, int xRange, int zRange) {
            IntBounds3 bounds = new IntBounds3(center, xRange, xRange, zRange);
            normalizeBounds(bounds);
            List<Vector3Int> positions = new List<Vector3Int>();
            bounds.iterate((x, y, z) => {
                int blockType = map.blockType.get(x, y, z);
                if (blockType != BlockTypeEnum.SPACE.CODE
                        && blockType != BlockTypeEnum.WALL.CODE
                        && blockType != BlockTypeEnum.FARM.CODE) { // passable position
                    positions.Add(new Vector3Int(x, y, z));
                }
            });
            return positions[Random.Range(0, positions.Count - 1)];
        }

        public IntBounds3 normalizeBounds(IntBounds3 bounds) {
            bounds.minX = Mathf.Max(bounds.minX, 0);
            bounds.maxX = Mathf.Min(bounds.maxX, map.xSize - 1);
            bounds.minY = Mathf.Max(bounds.minY, 0);
            bounds.maxY = Mathf.Min(bounds.maxY, map.ySize - 1);
            bounds.minZ = Mathf.Max(bounds.minZ, 0);
            bounds.maxZ = Mathf.Min(bounds.maxZ, map.zSize - 1);
            return bounds;
        }
    }
}