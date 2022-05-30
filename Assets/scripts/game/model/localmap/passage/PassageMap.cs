using System.Data;
using enums;
using types;
using UnityEngine;
using util;
using util.pathfinding;
using static types.BlockTypeEnum;
using static enums.PassageEnum;

namespace game.model.localmap.passage {
    // stores isolated areas on local map to enhance pathfinding
    public class PassageMap {
        private readonly LocalMap localMap;
        private readonly BlockTypeMap blockTypeMap;
        public readonly PassageUpdater updater;
        public readonly PassageUtil util;

        public UtilByteArrayWithCounter area; // number of area
        public UtilByteArray passage; // see {@link BlockTypesEnum} for passage values.

        public PassageMap(LocalMap localMap) {
            this.localMap = localMap;
            blockTypeMap = localMap.blockType;
            area = new UtilByteArrayWithCounter(localMap.sizeVector);
            passage = new UtilByteArray(localMap.sizeVector);
            updater = new PassageUpdater(localMap, this);
            util = new PassageUtil(localMap, this);
        }

        // Resets values to the whole map.
        public void init() {
            localMap.bounds.iterate((x, y, z) => passage.set(x, y, z, calculateTilePassage(x, y, z).VALUE));
            new AreaInitializer(localMap).formPassageMap(this);
        }

        // checks there is a walking path between two adjacent tiles
        public bool hasPathBetweenNeighbours(int x1, int y1, int z1, int x2, int y2, int z2) {
            if (!localMap.inMap(x1, y1, z1) || !localMap.inMap(x2, y2, z2) ||
                passage.get(x1, y1, z1) == IMPASSABLE.VALUE ||
                passage.get(x2, y2, z2) == IMPASSABLE.VALUE) return false; // out of map or not passable
            if (z1 == z2) return true; // passable tiles on same level
            int lowerType = z1 < z2 ? blockTypeMap.get(x1, y1, z1) : blockTypeMap.get(x2, y2, z2);
            if (lowerType == RAMP.CODE) {
                // ramp has space above
                if (x1 != x2 || y1 != y2)
                    return (z1 < z2 ? blockTypeMap.get(x1, y1, z1 + 1) : blockTypeMap.get(x2, y2, z2 + 1)) == SPACE.CODE;
            } else if (lowerType == STAIRS.CODE) {
                // stairs have stairs above
                if (x1 == x2 && y1 == y2) {
                    int upper = z1 > z2 ? blockTypeMap.get(x1, y1, z1) : blockTypeMap.get(x2, y2, z2);
                    return (upper == STAIRS.CODE || upper == DOWNSTAIRS.CODE) && lowerType == STAIRS.CODE; // handle stairs
                }
            }
            return false;
        }

        public bool tileIsAccessibleFromNeighbour(Vector3Int target, Vector3Int position, BlockType type)
            => tileIsAccessibleFromNeighbour(target.x, target.y, target.z, position.x, position.y, position.z, type);

        /**
         * Checks that unit, standing in position will have access (to dig, open a chest) to target tile.
         * Same Z-level tiles are always accessible.
         * Tiles are accessible vertically with stairs or ramps.
         */
        public bool tileIsAccessibleFromNeighbour(int tx, int ty, int tz, int x, int y, int z, BlockType targetType) {
            if (!localMap.inMap(tx, ty, tz) || !localMap.inMap(x, y, z) || passage.get(x, y, z) == IMPASSABLE.VALUE) return false;
            if (tz == z) return true;
            BlockType fromType = blockTypeMap.getEnumValue(x, y, z);
            BlockType lowerType = tz < z ? targetType : fromType;
            if (lowerType == RAMP) {
                // ramp has space above
                if (tx != x || ty != y)
                    return (tz < z ? blockTypeMap.get(tx, ty, tz + 1) : blockTypeMap.get(x, y, z + 1)) == SPACE.CODE;
            } else if (lowerType == STAIRS) {
                if (tx == x && ty == y) {
                    BlockType upperType = tz > z ? targetType : fromType;
                    return (upperType == STAIRS || upperType == DOWNSTAIRS) && lowerType == STAIRS; // handle stairs
                }
            }
            return false;
        }

        public bool tileIsAccessibleFromArea(int tx, int ty, int tz, int areaValue) {
            Debug.Log("checking [" + tx + ", " + ty + ", " + tz + "] available from area " + areaValue);
            if (!localMap.inMap(tx, ty, tz)) return false;
            if (area.get(tx, ty, tz) == areaValue) return true;
            if (getPassage(tx, ty, tz) == PASSABLE.VALUE) 
                throw new DataException("Passable tile + " + tx + " " + ty + " " + tz + " has no area value.");
            for (int x = tx - 1; x < tx + 2; x++) {
                for (int y = ty - 1; y < ty + 2; y++) {
                    if ((x != tx || y != ty) && localMap.inMap(x, y, tz) && area.get(x, y, tz) == areaValue)
                        return true;
                }
            }
            return false;
        }

        public bool tileIsAccessibleFromArea(Vector3Int target, Vector3Int source) =>
            tileIsAccessibleFromArea(target.x, target.y, target.z, area.get(source));

        public bool hasPathBetweenNeighbours(Vector3Int from, Vector3Int to)
            => hasPathBetweenNeighbours(@from.x, @from.y, @from.z, to.x, to.y, to.z);

        public Passage calculateTilePassage(Vector3Int position) => calculateTilePassage(position.x, position.y, position.z);

        // TODO
        public Passage calculateTilePassage(int x, int y, int z) {
            if (BlockTypeEnum.get(blockTypeMap.get(x, y, z)).PASSAGE == IMPASSABLE) return IMPASSABLE;
            if (!GameModel.get().plantContainer.isPlantBlockPassable(x, y, z)) return IMPASSABLE;

            bool buildingPassable = true;
            //        model.optional(BuildingContainer.class)
            //        .map(container -> container.buildingBlocks.get(position))
            //        .map(block -> block.passage == PASSABLE).orElse(true);
            if (!buildingPassable) return IMPASSABLE;

            bool waterPassable = true;
            //model.optional(LiquidContainer.class)
            //.map(container -> container.getTile(position))
            //.map(tile -> tile.amount <= 4).orElse(true);
            if (!waterPassable) return IMPASSABLE;

            return PassageEnum.PASSABLE;
        }

        public byte getPassage(int x, int y, int z) => passage.get(x, y, z);

        public bool inSameArea(Vector3Int pos1, Vector3Int pos2) {
            return localMap.inMap(pos1) && localMap.inMap(pos2) && area.get(pos1) == area.get(pos2);
        }
    }
}