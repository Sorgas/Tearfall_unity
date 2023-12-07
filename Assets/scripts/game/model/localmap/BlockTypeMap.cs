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

        private void set(int x, int y, int z, int blockType, int material) {
            Vector3Int position = new Vector3Int(x, y, z);
            if (!withinBounds(x, y, z)) return;
            int currentBlockType = get(x, y, z);
            setRaw(x, y, z, blockType, material);
            // update ramps if wall changed to non-wall or otherwise
            localMap.updateTile(position, (currentBlockType == WALL.CODE) != (blockType == WALL.CODE));
            // TODO destroy buildings if type != floor
            // TODO kill units if type == wall
            // walls create floors in above tile
            if (blockType == WALL.CODE && withinBounds(x, y, z + 1) && get(x, y, z + 1) == SPACE.CODE) {
                set(x, y, z + 1, FLOOR.CODE, this.material[x, y, z]);
            }
        }
        
        private void set(Vector3Int pos, int blockType, int material) {
            if (!withinBounds(pos.x, pos.y, pos.z)) return;
            int currentBlockType = get(pos.x, pos.y, pos.z);
            setRaw(pos.x, pos.y, pos.z, blockType, material);
            // update ramps if wall changed to non-wall or otherwise
            localMap.updateTile(pos, (currentBlockType == WALL.CODE) != (blockType == WALL.CODE));
            // TODO destroy buildings if type != floor
            // TODO kill units if type == wall
            // walls create floors in above tile
            if (blockType == WALL.CODE && withinBounds(pos.x, pos.y, pos.z + 1) && get(pos.x, pos.y, pos.z + 1) == SPACE.CODE) {
                set(pos.x, pos.y, pos.z + 1, FLOOR.CODE, this.material[pos.x, pos.y, pos.z]);
            }
        }
        
        public void setRaw(int x, int y, int z, int value, string newMaterial) => setRaw(x, y, z, value, MaterialMap.get().id(newMaterial));

        public void set(Vector3Int position, BlockType type, int newMaterial) => set(position, type.CODE, newMaterial);
        
        public void set(Vector3Int position, BlockType type) => set(position.x, position.y, position.z, type);
        
        private void set(int x, int y, int z, BlockType type) => set(x, y, z, type, getMaterial(x, y, z));

        public void set(int x, int y, int z, BlockType type, int newMaterial) => set(x, y, z, type.CODE, newMaterial);

        public int getMaterial(Vector3Int pos) => getMaterial(pos.x, pos.y, pos.z);

        public int getMaterial(int x, int y, int z) => withinBounds(x, y, z) ? material[x, y, z] : -1; // todo

        public BlockType getEnumValue(Vector3Int position) => getEnumValue(position.x, position.y, position.z);

        public BlockType getEnumValue(int x, int y, int z) => BlockTypes.get(get(x, y, z));

        // returns code of block
        public new int get(int x, int y, int z) {
            return withinBounds(x, y, z) ? base.get(x, y, z) : SPACE.CODE;
        }
    }
}