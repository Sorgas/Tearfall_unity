using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation.localgen;
using Assets.scripts.generation.worldgen;
using Assets.scripts.util.lang;
using Leopotam.Ecs;

namespace Assets.scripts.generation {
    // singleton to hold all data related to world and localmap generation
    // configs exist during all generations
    public class GenerationState : Singleton<GenerationState> {
        public WorldGenConfig worldGenConfig = new WorldGenConfig();
        public WorldGenSequence worldGenSequence; // runs generators
        public WorldGenContainer worldGenContainer; // intermediate results

        public LocalGenConfig localGenConfig = new LocalGenConfig();
        public LocalGenSequence localGenSequence; // runs generators
        public LocalGenContainer localGenContainer; // intermediate results
        public EcsWorld ecsWorld; // some local generators generate entities

        public PreparationState preparationState = new PreparationState(); // data from preparation screen (settlers, items, pets)

        public World world = new World();

        public void generateWorld() {
            worldGenSequence = new WorldGenSequence();
            worldGenContainer = worldGenSequence.run(worldGenConfig); // actual generation
            world.worldMap = worldGenContainer.createWorldMap();
        }

        public void generateLocalMap() {
            localGenContainer = new LocalGenContainer();
            localGenSequence = new LocalGenSequence();
            ecsWorld = new EcsWorld();
            world.localMap = localGenSequence.run();
        }
    }

    public class PreparationState {
        public List<SettlerData> settlers = new List<SettlerData>();
        public List<ItemData> items = new List<ItemData>();
    }

    // Descriptor for settler. Used to generate unit when game starts.
    public class SettlerData {
        public string name;
        public int age;
        // todo 
    }

    public class ItemData {
        public string type;
        public string material;
        public int quantity;
    }
}