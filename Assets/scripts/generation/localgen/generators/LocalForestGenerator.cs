using enums;
using game.model;
using game.model.localmap;
using generation.plant;
using Leopotam.Ecs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace generation.localgen.generators {
    // TODO add plant type selection based on position on world map
    public class LocalForestGenerator : LocalGenerator {
        private int maxAttempts = 20;
        private LocalMap map;
        private PlantGenerator generator = new();
         
        public override void generate() {
            Debug.Log("generating trees");
            map = GameModel.localMap;
            int treesNumber = config.areaSize * config.areaSize / 125 * config.forestationLevel;
            treesNumber += Random.Range(-1, 1) * 20;
            for (int i = 0; i < treesNumber; i++) {
                Vector3Int treePosition = findPlaceForTree();
                if(treePosition.x >= 0) createTree(treePosition);
            }
        }

        private Vector3Int findPlaceForTree() {
            for (int i = 0; i < maxAttempts; i++) {
                int x = Random.Range(0, map.bounds.maxX);
                int y = Random.Range(0, map.bounds.maxY);
                int z = findZ(x, y);
                Vector3Int position = new(x, y, z);
                if (z >= 0 && checkTreeOverlap(position)) return position;
            }
            return Vector3Int.left;
        }

        private int findZ(int x, int y) {
            for (int z = map.bounds.maxZ; z > 0; z--) {
                if (map.blockType.get(x, y, z) == BlockTypeEnum.FLOOR.CODE) {
                    return z;
                }
            }
            return -1;
        }

        // TODO check blocks of multi-tile trees
        private bool checkTreeOverlap(Vector3Int position) {
            return GameModel.get().plantContainer.getBlock(position) == null;
            // foreach (Vector3Int treePosition in treePositions) {
            //     if (Math.Abs(treePosition.x - x) < 3 || Math.Abs(treePosition.y - y) < 3) {
            //         return false;
            //     }
            // }
            // return true;
        }
        
        private void createTree(Vector3Int treePosition) {
            EcsEntity entity = generator.generate("oak"); // TODO
            GameModel.get().plantContainer.addPlant(entity, treePosition);
        }
        
        public override string getMessage() {
            return "Adding forests...";
        }
    }
}