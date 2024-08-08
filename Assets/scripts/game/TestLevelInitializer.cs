using game.model;
using generation;
using UnityEngine;

namespace game {
public class TestLevelInitializer {
    public void createTestLocalMap() {
        if (GameModel.get().world != null) {
            Debug.LogWarning("world already exists in GameModel");
            return;
        }
        new GenerationStateTestInitializer().initState();
        GenerationState.get().generateLocalMap("main");
    }
}
}