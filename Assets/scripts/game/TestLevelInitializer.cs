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
        int seed = System.DateTime.Now.Millisecond;
        Debug.Log($"Seed used for generation: {seed}");
        new GenerationStateTestInitializer().initState(seed);
        GenerationState.get().generateLocalMap("main");
    }
}
}