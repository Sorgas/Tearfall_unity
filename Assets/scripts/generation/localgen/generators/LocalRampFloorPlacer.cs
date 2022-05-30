using enums;
using game.model;
using game.model.localmap;
using types;
using UnityEngine;

namespace generation.localgen.generators {
    public class LocalRampFloorPlacer : LocalGenerator {
        private LocalMap localMap;
        private int wallCode = BlockTypeEnum.WALL.CODE;
        private int spaceCode = BlockTypeEnum.SPACE.CODE;
        private int rampCode = BlockTypeEnum.RAMP.CODE;

        public override void generate() {
            Debug.Log("placing ramps");
            localMap = GameModel.localMap;
            fillRamps();
            fillFloors();
        }

        private void fillRamps() {
            for (int x = 0; x <= localMap.bounds.maxX; x++) {
                for (int y = 0; y <= localMap.bounds.maxY; y++) {
                    for (int z = 1; z <= localMap.bounds.maxZ; z++) {
                        if (isGround(x, y, z) && hasAdjacentWall(x, y, z)) {
                            localMap.blockType.setRaw(x, y, z, BlockTypeEnum.RAMP.CODE, adjacentWallMaterial(x, y, z));
                        }
                    }
                }
            }
        }

        private void fillFloors() {
            for (int x = 0; x <= localMap.bounds.maxX; x++) {
                for (int y = 0; y <= localMap.bounds.maxY; y++) {
                    for (int z = localMap.bounds.maxZ - 1; z > 0; z--) {
                        if (isGround(x, y, z)) { //non space sell
                            localMap.blockType.setRaw(x, y, z, BlockTypeEnum.FLOOR.CODE, localMap.blockType.getMaterial(x, y, z - 1));
                        }
                    }
                }
            }
        }

        private bool isGround(int x, int y, int z) {
            return localMap.blockType.get(x, y, z) == spaceCode && localMap.blockType.get(x, y, z - 1) == wallCode;
        }

        private bool hasAdjacentWall(int xc, int yc, int z) {
            for (int x = xc - 1; x <= xc + 1; x++) {
                for (int y = yc - 1; y <= yc + 1; y++) {
                    if (localMap.inMap(x, y, z) && localMap.blockType.get(x, y, z) == wallCode) 
                        return true;
                }
            }
            return false;
        }

        private int adjacentWallMaterial(int xc, int yc, int z) {
            for (int x = xc - 1; x <= xc + 1; x++) {
                for (int y = yc - 1; y <= yc + 1; y++) {
                    if (localMap.inMap(x, y, z) && localMap.blockType.get(x, y, z) == wallCode) 
                        return localMap.blockType.getMaterial(x, y, z);
                }
            }
            return 0;
        }

        public override string getMessage() {
            return "making surface..";
        }
    }
}
