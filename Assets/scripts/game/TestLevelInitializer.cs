using game.model;
using generation;
using UnityEngine;

namespace game {
public class TestLevelInitializer {
    public void createTestLocalMap() {
        if (GameModel.get().worldModel != null) {
            Debug.LogWarning("world already exists in GameModel");
            return;
        }
        // new GenerationStateTestInitializer().initState();
        new GenerationStateTestInitializer().initCombatState();
        GenerationState.get().generateLocalMap("main");
    }
}
}