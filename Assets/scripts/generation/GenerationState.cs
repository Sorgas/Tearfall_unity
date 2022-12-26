using game.model;
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

        // generates WorldModel and sets it to Gamemodel
        public void generateWorld() {
            World world = new();
            worldGenContainer = new WorldGenContainer();
            worldGenSequence = new WorldGenSequence();
            worldGenSequence.run();
            WorldMap worldMap = worldGenContainer.createWorldMap();
            world.worldModel.worldMap = worldMap;
            GameModel.get().world = world;
        }
    }
}