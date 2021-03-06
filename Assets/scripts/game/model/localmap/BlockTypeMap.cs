﻿using Assets.scripts.enums;
using Assets.scripts.util;
using Assets.scripts.util.geometry;
using Tearfall_unity.Assets.scripts.enums.material;
using UnityEngine;
using static Assets.scripts.enums.BlockTypeEnum;

namespace Assets.scripts.game.model.localmap {
    // stores tile types and materials for local map
    public class BlockTypeMap : UtilByteArray {
        public int[,,] material;
        private IntVector3 cachePosition;
        private LocalMap localMap;

        public BlockTypeMap(LocalMap localMap) : base(localMap.xSize, localMap.ySize, localMap.zSize) {
            this.localMap = localMap;
            material = new int[localMap.xSize, localMap.ySize, localMap.zSize];
            cachePosition = new IntVector3();
        }

        // set block type without maintaining tile consistency.
        public void setRaw(int x, int y, int z, int value, int material) {
            if(!withinBounds(x,y,z)) return;
            this.material[x, y, z] = material;
            base.set(x, y, z, value);
        }

        public void set(int x, int y, int z, int value, int material) {
            if(!withinBounds(x,y,z)) return;
            int currentBlock = get(x, y, z);
            bool updateRamps = currentBlock != value && (currentBlock == RAMP.CODE || value == RAMP.CODE);
            setRaw(x, y, z, value, material);
            localMap.updateTile(cachePosition.set(x, y, z), updateRamps);
            // TODO destroy buildings if type != floor
            // TODO kill units if type == wall
            if (value == WALL.CODE && withinBounds(x, y, z + 1) && get(x, y, z + 1) == SPACE.CODE) {
                set(x, y, z + 1, FLOOR.CODE, this.material[x, y, z]);
            }
        }

        public new void setRaw(int x, int y, int z, int value) => setRaw(x, y, z, value, getMaterial(x, y, z));

        public void setRaw(int x, int y, int z, int value, string material) => setRaw(x, y, z, value, MaterialMap.get().id(material));

        public new void set(int x, int y, int z, int value) => set(x, y, z, value, getMaterial(x, y, z));

        public void set(IntVector3 position, BlockType type) => set(position.x, position.y, position.z, type);

        public void set(int x, int y, int z, BlockType type) => set(x, y, z, type, getMaterial(x, y, z));

        public void set(IntVector3 position, BlockType type, int material) => set(position, type, material);

        public void set(int x, int y, int z, BlockType type, int material) => set(x, y, z, type.CODE, material);

        public int getMaterial(Vector3Int pos) => getMaterial(pos.x, pos.y, pos.z);

        public int getMaterial(int x, int y, int z) {
            return withinBounds(x, y, z) ? material[x, y, z] : -1; // todo
        }

        public BlockType getEnumValue(Vector3Int position) => getEnumValue(position.x, position.y, position.z);

        public BlockType getEnumValue(int x, int y, int z) {
            return BlockTypeEnum.get(get(x, y, z));
        }

        // returns code of block
        public new int get(int x, int y, int z) {
            return withinBounds(x, y, z) ? base.get(x, y, z) : SPACE.CODE;
        }
    }
}
