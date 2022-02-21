using game.model;
using generation.localgen;
using generation.worldgen;
using Leopotam.Ecs;
using util.lang;

namespace generation {
    // singleton to hold all data related to world and localmap generation
    // configs exist during all generations
    public class GenerationState : Singleton<GenerationState> {
        public WorldGenConfig worldGenConfig = new WorldGenConfig();
        public WorldGenSequence worldGenSequence;
        public WorldGenContainer worldGenContainer;

        public PreparationState preparationState = new PreparationState(); // data from preparation screen (settlers, items, pets)

        public LocalGenConfig localGenConfig = new LocalGenConfig();
        public LocalGenSequence localGenSequence;
        public LocalGenContainer localGenContainer;

        public bool ready = false;

        public void generateWorld() {
            worldGenContainer = new WorldGenContainer();
            worldGenSequence = new WorldGenSequence();
            worldGenSequence.run();
            GameModel.get().world.worldMap = worldGenContainer.createWorldMap();
        }

        public void generateLocalMap() {
            localGenContainer = new LocalGenContainer();
            localGenSequence = new LocalGenSequence();
            localGenSequence.run();
            ready = true;
        }
    }
}