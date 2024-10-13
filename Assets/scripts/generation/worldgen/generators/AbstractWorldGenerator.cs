using UnityEngine;
using Random = System.Random;

namespace generation.worldgen.generators {
// Abstract world generator. Generators should not use GameModel.
public abstract class AbstractWorldGenerator {
    protected WorldGenConfig config;
    protected WorldGenContainer container;
    protected string name = "WorldGenerator";
    protected bool debug = false;
    protected Random numberGenerator;

    public void generate() {
        bindStateObjects();
        generateInternal();
    }

    // Data used for generation should be refreshed for each generation
    private void bindStateObjects() {
        container = GenerationState.get().worldGenerator.worldGenContainer;
        numberGenerator = GenerationState.get().worldGenerator.worldGenSequence.random;
        config = GenerationState.get().worldGenerator.worldGenConfig;
    }
    
    protected abstract void generateInternal();

    protected int random(int min, int max) {
        return numberGenerator.Next(min, max);
    }

    protected void log(string message) {
        if (debug) Debug.Log($"[{name}]: {message}");
    }
}
}