using Leopotam.Ecs;

namespace game.model.component.plant {
    public class PlantBlock {
        public EcsEntity plant;
        public readonly int material;
        public readonly int blockType; // type from enum
        public bool harvested;

        public PlantBlock(int material, int blockType) {
            this.material = material;
            this.blockType = blockType;
        }

        public bool isPassable() {
            //   return PlantBlocksTypeEnum.getType(blockType).passable;
            return true;
        }

        //public PlantBlocksTypeEnum getType() {
        //    return PlantBlocksTypeEnum.getType(blockType);
        //}
    }
}
