using game.model;
using generation;
using UnityEngine;
using util.geometry;

namespace game {
    public class TestLevelInitializer {
        public void createTestLocalMap() {
            if(GameModel.get().world != null && GameModel.get().world.localMap != null) {
                Debug.LogWarning("world already exists in GameModel");
                return;
            }
            GenerationState state = GenerationState.get();
            state.worldGenConfig.size = 10;
            state.generateWorld();
            createTestSettler();
            createTestItem();
            state.localGenConfig.location = new IntVector2(5, 5);
            state.generateLocalMap();
        }

        // creates test settler as it was selected on preparation screen
        private void createTestSettler() {
            GenerationState.get().preparationState.settlers.Add(new SettlerData {name = "settler", age = 30, type = "human"});
        }

        // creates test item as it was selected on preparation screen
        private void createTestItem() {
            GenerationState.get().preparationState.items.Add(new ItemData {material = "iron", type = "pickaxe", quantity = 1});
            GenerationState.get().preparationState.items.Add(new ItemData {material = "cotton", type = "pants", quantity = 1});
        }
    }
}