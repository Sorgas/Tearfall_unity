using UnityEngine;
using Random = System.Random;

namespace generation.worldgen.generators {
// Abstract world generator. Generators should not use GameModel.
public abstract class WorldGenerator {
    protected WorldGenConfig config;
    protected WorldGenContainer container;
    protected string name = "WorldGenerator";
    protected bool debug = false;
    private Random numberGenerator;

    public void generate() {
        container = GenerationState.get().worldGenContainer;
        numberGenerator = GenerationState.get().worldGenSequence.random;
        config = GenerationState.get().worldGenConfig;
        generateInternal();
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