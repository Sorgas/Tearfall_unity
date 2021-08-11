using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation.localgen;
using Assets.scripts.generation.worldgen;
using Assets.scripts.util.lang;

namespace Assets.scripts.generation {
    // singleton to hold all data related to world and localmap generation
    // configs exist during all generations
    public class GenerationState : Singleton<GenerationState> {
        public WorldGenConfig worldGenConfig = new WorldGenConfig();
        public WorldGenSequence worldGenSequence;
        public WorldGenContainer worldGenContainer;

        public LocalGenConfig localGenConfig = new LocalGenConfig();
        public LocalGenSequence localGenSequence;
        public LocalGenContainer localGenContainer;

        public StartGameData startGameData = new StartGameData();

        public World world = new World();

        public void generateWorld() {
            worldGenSequence = new WorldGenSequence();
            worldGenContainer = worldGenSequence.run(worldGenConfig); // actual generation
            world.worldMap = worldGenContainer.createWorldMap();
        }

        public LocalMap generateLocalMap() {
            localGenContainer = new LocalGenContainer();
            localGenSequence = new LocalGenSequence();
            return localGenSequence.run();
        }
    }

    public class StartGameData {
        public List<SettlerData> settlers = new List<SettlerData>();
    }

    // Descriptor for settler. Used to generate unit when game starts.
    public class SettlerData {
        public string name;
        public int age;
        // todo 
    }
}