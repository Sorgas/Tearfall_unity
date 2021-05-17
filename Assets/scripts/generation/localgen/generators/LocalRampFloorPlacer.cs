using Assets.scripts.enums;
using Assets.scripts.game.model.localmap;
using UnityEngine;

namespace Assets.scripts.generation.localgen.generators {
    public class LocalRampFloorPlacer : LocalGenerator {
        private LocalMap localMap;
        private int wallCode = BlockTypeEnum.WALL.CODE;
        private int spaceCode = BlockTypeEnum.SPACE.CODE;
        private int rampCode = BlockTypeEnum.RAMP.CODE;

        public override void generate() {
            Debug.Log("placing ramps");
            localMap = GenerationState.get().localGenContainer.localMap;
            fillRamps();
            fillFloors();
        }

        private void fillRamps() {
            for (int x = 0; x < localMap.xSize; x++) {
                for (int y = 0; y < localMap.xSize; y++) {
                    for (int z = 1; z < localMap.zSize; z++) {
                        if (isGround(x, y, z) && hasAdjacentWall(x, y, z)) {
                            Debug.Log("ramp set");
                            localMap.blockType.set(x, y, z, (byte)rampCode, adjacentWallMaterial(x, y, z));
                        }
                    }
                }
            }
        }

        private void fillFloors() {
            for (int x = 0; x < localMap.xSize; x++) {
                for (int y = 0; y < localMap.ySize; y++) {
                    for (int z = localMap.zSize - 1; z > 0; z--) {
                        if (isGround(x, y, z)) { //non space sell
                            localMap.blockType.set(x, y, z, BlockTypeEnum.FLOOR.CODE, localMap.blockType.getMaterial(x, y, z - 1));
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
                    if (localMap.inMap(x, y, z) && localMap.blockType.get(x, y, z) == wallCode) return true;
                }
            }
            return false;
        }

        private int adjacentWallMaterial(int xc, int yc, int z) {
            for (int x = xc - 1; x <= xc + 1; x++) {
                for (int y = yc - 1; y <= yc + 1; y++) {
                    if (localMap.inMap(x, y, z) && localMap.blockType.get(x, y, z) == wallCode) return localMap.blockType.getMaterial(x, y, z);
                }
            }
            return 0;
        }
    }
}
