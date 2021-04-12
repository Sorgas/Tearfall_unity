using Assets.scripts.enums;
using Assets.scripts.util;
using Assets.scripts.util.geometry;
using UnityEngine;
using static Assets.scripts.enums.BlockTypeEnum;

namespace Assets.scripts.game.model.localmap {
    public class BlockTypeMap : UtilByteArray {
        public int[,,] material;
        private IntVector3 cachePosition;
        private LocalMap localMap;

        public BlockTypeMap(LocalMap localMap) : base(localMap.xSize, localMap.ySize, localMap.zSize) {
            this.localMap = localMap;
            material = new int[localMap.xSize, localMap.ySize, localMap.zSize];
            cachePosition = new IntVector3();
        }

        public new void set(int x, int y, int z, int value) {
            base.set(x, y, z, value);
            // if (value == WALL.CODE && withinBounds(x, y, z + 1) && get(x, y, z + 1) == SPACE.CODE) {
            //     setBlock(x, y, z + 1, FLOOR.CODE, material[x, y, z]);
            // }
            cachePosition.set(x, y, z);
            // if (value != FARM.CODE && value != FLOOR.CODE) { // remove plants if block becomes unsuitable for plants
            //     //GameModel.get(PlantContainer).removeBlock(cachePosition, false);
            // }
            // TODO destroy buildings if type != floor
            // TODO kill units if type == wall
            localMap.updateTile(cachePosition);
        }

        public void set(int x, int y, int z, BlockType type) {
            set(x, y, z, type.CODE);
        }

        public void set(IntVector3 position, BlockType type) {
            set(position.x, position.y, position.z, type);
        }

        public void setBlock(IntVector3 position, BlockType type, int material) {
            setBlock(position, type.CODE, material);
        }

        public void setBlock(int x, int y, int z, BlockType type, int material) {
            setBlock(x, y, z, type.CODE, material);
        }


        public void setBlock(IntVector3 position, int type, int material) {
            setBlock(position.x, position.y, position.z, type, material);
        }

        public void setBlock(int x, int y, int z, int type, int material) {
            this.material[x, y, z] = material;
            set(x, y, z, type);
        }

        public int getMaterial(IntVector3 pos) {
            return material[pos.x, pos.y, pos.z];
        }

        public int getMaterial(int x, int y, int z) {
            return material[x, y, z];
        }

        public BlockType getEnumValue(IntVector3 position) {
            return getEnumValue(position.x, position.y, position.z);
        }

        public BlockType getEnumValue(int x, int y, int z) {
            return BlockTypeEnum.get(get(x, y, z));
        }

        public new int get(int x, int y, int z) {
            return withinBounds(x, y, z) ? base.get(x, y, z) : SPACE.CODE;
        }
    }
}
