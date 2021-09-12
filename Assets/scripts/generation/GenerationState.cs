using Assets.scripts.game.model;
using Assets.scripts.generation.localgen;
using Assets.scripts.generation.worldgen;
using Assets.scripts.util.lang;
using Leopotam.Ecs;
using Tearfall_unity.Assets.scripts.generation;

namespace Assets.scripts.generation {
    // singleton to hold all data related to world and localmap generation
    // configs exist during all generations
    public class GenerationState : Singleton<GenerationState> {
        public EcsWorld ecsWorld; // some local generators generate entities
        public World world = new World();

        public WorldGenConfig worldGenConfig = new WorldGenConfig();
        public WorldGenSequence worldGenSequence;
        public WorldGenContainer worldGenContainer;

        public PreparationState preparationState = new PreparationState(); // data from preparation screen (settlers, items, pets)

        public LocalGenConfig localGenConfig = new LocalGenConfig();
        public LocalGenSequence localGenSequence;
        public LocalGenContainer localGenContainer;

        public bool ready = false;

        public void generateWorld() {
            if(ecsWorld != null) ecsWorld.Destroy();
            ecsWorld = new EcsWorld();
            worldGenContainer = new WorldGenContainer();
            worldGenSequence = new WorldGenSequence();
            worldGenSequence.run();
            world.worldMap = worldGenContainer.createWorldMap();
        }

        public void generateLocalMap() {
            localGenContainer = new LocalGenContainer();
            localGenSequence = new LocalGenSequence();
            localGenSequence.run();
            world.localMap = localGenContainer.localMap;
            ready = true;
        }
    }
}