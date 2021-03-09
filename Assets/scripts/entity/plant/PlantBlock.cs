using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.entity.plant {
    class PlantBlock {
        public AbstractPlant plant;
        public readonly int material;
        public readonly int blockType; // type from enum
        public readonly int[] atlasXY;
        public bool harvested;

        public PlantBlock(int material, int blockType) {
            this.material = material;
            this.blockType = blockType;
            atlasXY = new int[2];
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
