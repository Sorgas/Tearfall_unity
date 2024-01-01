using game.model.container;
using types;
using UnityEngine;
using util;
using static types.BlockTypes;
using static types.PassageTypes;

namespace game.model.localmap.passage {
// Stores passage values of map tiles.
// Has different PassageHelpers for different moving units (settlers, animals, TODO flyers)
    public class PassageMap : LocalModelContainer {
        private readonly LocalMap localMap;
        private readonly BlockTypeMap map;
        private bool inited = false;
        // public readonly PassageUtil util;

        public DefaultPassageHelper defaultHelper;
        public GroundNoDoorsPassageHelper groundNoDoorsHelper;
        public IndoorPassageHelper indoorHelper;
        // public RoomPassageHelper roomHelper;
        public readonly UtilByteArray passage; // see {@link BlockTypesEnum} for passage values.
        
        public PassageMap(LocalModel model, LocalMap localMap) : base(model) {
            this.localMap = localMap;
            map = localMap.blockType;
            passage = new UtilByteArray(localMap.sizeVector);
        }

        public void init() {
            localMap.bounds.iterate(position => passage.set(position, calculateTilePassage(position).VALUE));
            defaultHelper = new(this, model);
            groundNoDoorsHelper = new(this, model);
            indoorHelper = new(this, model);
            // roomHelper = new(this, model);
            inited = true;
        }

        public void update(Vector3Int position) {
            if (!inited) return; 
            passage.set(position, calculateTilePassage(position).VALUE);
            defaultHelper.update(position);
            groundNoDoorsHelper.update(position);
            indoorHelper.update(position);
            // roomHelper.update(position);
        }

        // TODO remove
        public bool hasPathBetweenNeighboursWithOverride(Vector3Int target, Vector3Int position, BlockType type)
            => hasPathBetweenNeighboursWithOverride(target.x, target.y, target.z, position.x, position.y, position.z, type);
        
        // /**
        //  * Checks that unit, standing in position will have access (to dig, open a chest) to target tile.
        //  * Same Z-level tiles are always accessible.
        //  * Tiles are accessible vertically with stairs or ramps.
        //  */
        // TODO remove
        public bool hasPathBetweenNeighboursWithOverride(int tx, int ty, int tz, int x, int y, int z, BlockType targetType) {
            if (!localMap.inMap(tx, ty, tz) || !localMap.inMap(x, y, z) || passage.get(x, y, z) == IMPASSABLE.VALUE) return false;
            if (tz == z) return true;
            BlockType fromType = map.getEnumValue(x, y, z);
            BlockType lowerType = tz < z ? targetType : fromType;
            if (lowerType == RAMP) {
                // ramp has space above
                if (tx != x || ty != y)
                    return (tz < z ? map.get(tx, ty, tz + 1) : map.get(x, y, z + 1)) == SPACE.CODE;
            } else if (lowerType == STAIRS) {
                if (tx == x && ty == y) {
                    BlockType upperType = tz > z ? targetType : fromType;
                    return (upperType == STAIRS || upperType == DOWNSTAIRS) && lowerType == STAIRS; // handle stairs
                }
            }
            return false;
        }
        
        // TODO implement other checks (water)
        public Passage calculateTilePassage(Vector3Int position) {
            Passage blockPassage = BlockTypes.get(map.get(position)).PASSAGE;
            if (blockPassage == IMPASSABLE) return IMPASSABLE; // nothing can make impassable block passable
            if (!model.plantContainer.isPlantBlockPassable(position)) return IMPASSABLE; // plant can make tile impassable
            if (model.buildingContainer.hasBuilding(position)) { 
                return model.buildingContainer.getBuildingBlockPassage(position);
            }
            // bool waterPassable = true;
            //model.optional(LiquidContainer.class)
            //.map(container -> container.getTile(position))
            //.map(tile -> tile.amount <= 4).orElse(true);
            // if (!waterPassable) return IMPASSABLE;
            return blockPassage;
        }

        public byte getPassage(int x, int y, int z) => passage.get(x, y, z);

        public byte getPassage(Vector3Int position) => getPassage(position.x, position.y, position.z);

        public Passage getPassageType(Vector3Int position) => PassageTypes.get(passage.get(position));

        public bool inSameArea(Vector3Int pos1, Vector3Int pos2) {
            return localMap.inMap(pos1) && localMap.inMap(pos2) && defaultHelper.area.get(pos1) == defaultHelper.area.get(pos2);
        }
    }
}