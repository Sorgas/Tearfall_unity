using enums;
using UnityEngine;
using util;
using util.pathfinding;
using static enums.BlockTypeEnum;
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

        /**
         * Checks that walking creature can move from one tile to another.
         * Tiles should be adjacent.
         */
        public bool hasPathBetweenNeighbours(int x1, int y1, int z1, int x2, int y2, int z2) {
            if (!localMap.inMap(x1, y1, z1) || !localMap.inMap(x2, y2, z2)) return false; // out of map
            if (passage.get(x1, y1, z1) == IMPASSABLE.VALUE || passage.get(x2, y2, z2) == IMPASSABLE.VALUE) return false;
            if (z1 == z2) return true; // passable tiles on same level
            int lower = z1 < z2 ? blockTypeMap.get(x1, y1, z1) : blockTypeMap.get(x2, y2, z2);
            if (x1 != x2 || y1 != y2) return lower == RAMP.CODE; // handle ramps
            int upper = z1 > z2 ? blockTypeMap.get(x1, y1, z1) : blockTypeMap.get(x2, y2, z2);
            return (upper == STAIRS.CODE || upper == DOWNSTAIRS.CODE) && lower == STAIRS.CODE; // handle stairs
        }

        /**
         * Checks that unit, standing in position will have access (to dig, open a chest) to target tile.
         * Same Z-level tiles are always accessible.
         * Tiles are accessible vertically with stairs or ramps.
         */
        public bool tileIsAccessibleFromNeighbour(int targetX, int targetY, int targetZ, int x, int y, int z, BlockType targetType) {
            if (!localMap.inMap(targetX, targetY, targetZ) || !localMap.inMap(x, y, z) || passage.get(x, y, z) == IMPASSABLE.VALUE) return false;
            if (targetZ == z) return true;
            BlockType fromType = BlockTypeEnum.get(blockTypeMap.get(x, y, z));
            BlockType lower = targetZ < z ? targetType : fromType;
            if ((targetX == x) != (targetY == y)) return lower == RAMP; // ramp near and lower
            if (targetX != x) return false;
            BlockType upper = targetZ > z ? targetType : fromType;
            return lower == STAIRS && (upper == STAIRS || upper == DOWNSTAIRS);
        }

        public bool tileIsAccessibleFromNeighbour(Vector3Int target, Vector3Int position, BlockType type) 
            => tileIsAccessibleFromNeighbour(target.x, target.y, target.z, position.x, position.y, position.z, type);

        public bool hasPathBetweenNeighbours(Vector3Int from, Vector3Int to) 
            => hasPathBetweenNeighbours(@from.x, @from.y, @from.z, to.x, to.y, to.z);

        public Passage calculateTilePassage(Vector3Int position) => calculateTilePassage(position.x, position.y, position.z);

        public Passage calculateTilePassage(int x, int y, int z) {
            Passage tilePassage = BlockTypeEnum.get(blockTypeMap.get(x, y, z)).PASSAGE;
            // TODO
            if (tilePassage == PASSABLE) { // tile still can be blocked by plants or buildings
                bool plantPassable = true;
                //  GameModel.map(plantContainer => plantContainer.isPlantBlockPassable(position)).orElse(true);
                if (!plantPassable) return IMPASSABLE;

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
            }
            return tilePassage;
        }

        public byte getPassage(int x, int y, int z) => passage.get(x, y, z);

        public bool inSameArea(Vector3Int pos1, Vector3Int pos2) {
            return localMap.inMap(pos1) && localMap.inMap(pos2) && area.get(pos1) == area.get(pos2);
        }
    }
}