using System;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation.localgen;
using Assets.scripts.generation.worldgen;
using Assets.scripts.util.geometry;
using Assets.scripts.util.lang;

namespace Assets.scripts.generation {
    // singleton to hold all related to world and localmap generation
    public class GenerationState : Singleton<GenerationState> {
        public WorldGenSequence worldGenSequence;
        public WorldGenConfig worldGenConfig;
        public WorldGenContainer worldGenContainer;
        public World world;

        public LocalGenSequence localGenSequence;
        public LocalGenConfig localGenConfig;
        public LocalGenContainer localGenContainer;

        public void generateWorld(int seed, int size) {
            worldGenSequence = new WorldGenSequence();
            worldGenConfig = new WorldGenConfig(seed, size);
            worldGenContainer = worldGenSequence.run(worldGenConfig); // actual generation
            world = new World(worldGenContainer.createWorldMap());
        }

        public void setLocalPosition(IntVector2 position) {
            localGenConfig = new LocalGenConfig();
            localGenConfig.location = position;
        }

        public LocalMap generateLocalMap() {
            localGenContainer = new LocalGenContainer();
            localGenSequence = new LocalGenSequence();
            return localGenSequence.run();
        }
    }
}