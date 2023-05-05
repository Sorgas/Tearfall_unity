using game.model.localmap;
using generation.plant;
using Leopotam.Ecs;
using types;
using UnityEngine;

namespace generation.localgen.generators {
    public class LocalPlantGenerator : LocalGenerator {
        private int maxAttempts = 20;
        private LocalMap map;
        private PlantGenerator generator = new();

        public LocalPlantGenerator(LocalMapGenerator generator) : base(generator) { }
        
        public override void generate() {
            map = container.map;
            int treesNumber = config.areaSize * config.areaSize / 125 * config.forestationLevel;
            treesNumber += Random.Range(-1, 1) * 20;
            for (int i = 0; i < treesNumber; i++) {
                Vector3Int treePosition = findPlaceForTree();
                if(treePosition.x >= 0) createPlant(treePosition);
            }
        }
        
        private Vector3Int findPlaceForTree() {
            for (int i = 0; i < maxAttempts; i++) {
                int x = Random.Range(0, map.bounds.maxX);
                int y = Random.Range(0, map.bounds.maxY);
                int z = findZ(x, y);
                Vector3Int position = new(x, y, z);
                if (z >= 0 && checkPlantOverlap(position)) return position;
            }
            return Vector3Int.left;
        }

        private int findZ(int x, int y) {
            for (int z = map.bounds.maxZ; z > 0; z--) {
                if (map.blockType.get(x, y, z) == BlockTypes.FLOOR.CODE) {
                    return z;
                }
            }
            return -1;
        }

        // TODO check blocks of multi-tile trees
        private bool checkPlantOverlap(Vector3Int position) {
            return container.model.plantContainer.getBlock(position) == null;
        }
        
        private void createPlant(Vector3Int treePosition) {
            EcsEntity entity = generator.generate("oak", container.model.createEntity()); // TODO
            container.model.plantContainer.addPlant(entity, treePosition);
        }

        public override string getMessage() {
            return "generating plants...";
        }
    }
}