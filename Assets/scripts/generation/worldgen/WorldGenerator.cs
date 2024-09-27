using game.model;

namespace generation.worldgen {
// Root class for generating worlds. 
public class WorldGenerator {
    public readonly WorldGenConfig worldGenConfig = new(); // configured externally before generation
    public readonly WorldGenSequence worldGenSequence = new();
    public WorldGenContainer worldGenContainer;

    public void generateWorld() {
        worldGenContainer = new WorldGenContainer(worldGenConfig);
        worldGenSequence.run();
        GameModel.get().worldModel = worldGenContainer.createWorldModel();
    }
}
}