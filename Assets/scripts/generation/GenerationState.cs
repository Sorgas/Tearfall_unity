using game.model;
using game.model.localmap;
using generation.localgen;
using generation.worldgen;
using UnityEngine;
using util.lang;

namespace generation {
    // singleton to hold all objects related to world and localmap generation
    // configs exist during all generations
    public class GenerationState : Singleton<GenerationState> {
        public readonly WorldGenConfig worldGenConfig = new(); // configured externally before generation
        public readonly WorldGenSequence worldGenSequence = new();
        public WorldGenContainer worldGenContainer;
        
        public readonly PreparationState preparationState = PreparationState.get(); // data from preparation screen (settlers, items, pets)
        public LocalMapGenerator localMapGenerator = new();
        
        // generates WorldModel and sets it to GameModel
        public void generateWorld() {
            World world = new();
            worldGenContainer = new WorldGenContainer();
            worldGenSequence.run();
            world.name = worldGenContainer.worldName;
            WorldMap worldMap = worldGenContainer.createWorldMap();
            world.worldModel.worldMap = worldMap;
            GameModel.get().world = world; // first instantiation of GameModel // TODO move to separate method (update worldgen stage)
        }

        public void generateLocalMap(string name) {
            localMapGenerator.localGenConfig.location = preparationState.location;
            localMapGenerator.localGenConfig.areaSize = preparationState.size;
            localMapGenerator.localGenConfig.seed = getLocalMapSeed(worldGenConfig.seed, preparationState.location);
            LocalModel localModel = localMapGenerator.generateLocalMap(name);
            GameModel.get().addLocalModel(name, localModel);
        }

        public void generateFlatLocalMap(string name) {
            localMapGenerator.localGenConfig.location = preparationState.location;
            localMapGenerator.localGenConfig.areaSize = preparationState.size;
            localMapGenerator.localGenConfig.seed = getLocalMapSeed(worldGenConfig.seed, preparationState.location);
            LocalModel localModel = localMapGenerator.generateLocalMap(name);
            GameModel.get().addLocalModel(name, localModel);
        }

        private int getLocalMapSeed(int seed, Vector2Int location) {
            return seed + location.x * 100 + location.y * 100000;
        }
    }
}