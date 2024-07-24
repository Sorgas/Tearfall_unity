using game.model;
using game.model.localmap;
using generation.localgen;
using generation.worldgen;
using util.lang;

namespace generation {
    // singleton to hold all data related to world and localmap generation
    // configs exist during all generations
    public class GenerationState : Singleton<GenerationState> {
        public WorldGenConfig worldGenConfig = new();
        public WorldGenSequence worldGenSequence;
        public WorldGenContainer worldGenContainer;
        
        public PreparationState preparationState = new(); // data from preparation screen (settlers, items, pets)
        public LocalMapGenerator localMapGenerator = new();
        
        // generates WorldModel and sets it to GameModel
        public void generateWorld() {
            World world = new();
            worldGenContainer = new WorldGenContainer();
            worldGenSequence = new WorldGenSequence();
            worldGenSequence.run();
            world.name = worldGenContainer.worldName;
            WorldMap worldMap = worldGenContainer.createWorldMap();
            world.worldModel.worldMap = worldMap;
            GameModel.get().world = world; // first instantiating on GameModel
        }

        public void generateLocalMap(string name) {
            localMapGenerator.localGenConfig.location = preparationState.location;
            localMapGenerator.localGenConfig.areaSize = preparationState.size;
            LocalModel localModel = localMapGenerator.generateLocalMap(name);
            GameModel.get().addLocalModel(name, localModel);
        }

        public void generateFlatLocalMap(string name) {
            localMapGenerator.localGenConfig.location = preparationState.location;
            localMapGenerator.localGenConfig.areaSize = preparationState.size;
            LocalModel localModel = localMapGenerator.generateLocalMap(name);
            GameModel.get().addLocalModel(name, localModel);
        }
    }
}