using types;
using types.material;
using UnityEngine;
using util;
using static types.BlockTypes;

namespace game.model.localmap {
    // stores tile types and materials for local map
    public class BlockTypeMap : UtilByteArray {
        public int[,,] material;
        private LocalMap localMap;

        public BlockTypeMap(LocalMap localMap) : base(localMap.sizeVector) {
            this.localMap = localMap;
            material = new int[localMap.sizeVector.x, localMap.sizeVector.y, localMap.sizeVector.z];
        }

        // set block type without maintaining tile consistency.
        public void setRaw(int x, int y, int z, int value, int material) {
            if (!withinBounds(x, y, z)) return;
            this.material[x, y, z] = material;
            base.set(x, y, z, value);
        }

        public void set(int x, int y, int z, int blockType, int material) {
            if (!withinBounds(x, y, z)) return;
            int currentBlockType = get(x, y, z);
            setRaw(x, y, z, blockType, material);
            // update ramps if wall changed to non-wall or otherwise
            localMap.updateTile(x, y, z, (currentBlockType == WALL.CODE) != (blockType == WALL.CODE));
            // TODO destroy buildings if type != floor
            // TODO kill units if type == wall
            // set floor above
            if (blockType == WALL.CODE && withinBounds(x, y, z + 1) && get(x, y, z + 1) == SPACE.CODE) {
                set(x, y, z + 1, FLOOR.CODE, this.material[x, y, z]);
            }
        }

        public void setRaw(int x, int y, int z, int value) => setRaw(x, y, z, value, getMaterial(x, y, z));

        public void setRaw(int x, int y, int z, int value, string material) => setRaw(x, y, z, value, MaterialMap.get().id(material));

        public void set(Vector3Int position, BlockType type, int material) => set(position.x, position.y, position.z, type, material);
        
        public void set(Vector3Int position, BlockType type) => set(position.x, position.y, position.z, type);
        
        public new void set(int x, int y, int z, int blockType) => set(x, y, z, blockType, getMaterial(x, y, z));
        
        public void set(int x, int y, int z, BlockType type) => set(x, y, z, type, getMaterial(x, y, z));

        public void set(int x, int y, int z, BlockType type, int material) => set(x, y, z, type.CODE, material);

        public int getMaterial(Vector3Int pos) => getMaterial(pos.x, pos.y, pos.z);

        public int getMaterial(int x, int y, int z) {
            return withinBounds(x, y, z) ? material[x, y, z] : -1; // todo
        }

        public BlockType getEnumValue(Vector3Int position) => getEnumValue(position.x, position.y, position.z);

        public BlockType getEnumValue(int x, int y, int z) {
            return BlockTypes.get(get(x, y, z));
        }

        // returns code of block
        public new int get(int x, int y, int z) {
            return withinBounds(x, y, z) ? base.get(x, y, z) : SPACE.CODE;
        }
    }
}