using System;
using generation.worldgen.generators;
using generation.worldgen.generators.drainage;
using generation.worldgen.generators.elevation;
using generation.worldgen.generators.temperature;

namespace generation.worldgen {
// Stores all generators for world generation and executes them in correct order.
// all generators share same System.Random object
public class WorldGenSequence {
    private WorldNameGenerator nameGenerator;
    private WorldElevationGenerator elevationGenerator;
    private WorldOceanFiller oceanFiller;
    private WorldTemperatureGenerator temperatureGenerator;
    private RainfallGenerator rainfallGenerator;
    // private ErosionGenerator erosionGenerator;
    // private ElevationModifier elevationModifier;
    // private RiverGenerator riverGenerator;
    // private LakeGenerator lakeGenerator;
    // private BrookGenerator brookGenerator;
    // private DrainageGenerator drainageGenerator;
    // private BiomeGenerator biomeGenerator;
    // private CelestialBodiesGenerator celestialBodiesGenerator;
    private WorldFactionGenerator factionGenerator;
    public Random random; // shared to generators
    
    public WorldGenSequence() {
        nameGenerator = new WorldNameGenerator();
        elevationGenerator = new WorldElevationGenerator();
        oceanFiller = new WorldOceanFiller();
        temperatureGenerator = new WorldTemperatureGenerator();
        rainfallGenerator = new RainfallGenerator();
        factionGenerator = new WorldFactionGenerator();
    }

    public void run() {
        random = new Random(GenerationState.get().worldGenerator.worldGenConfig.seed);
        nameGenerator.generate();
        elevationGenerator.generate(); // generates elevation [0, 1]
        // celestialBodiesGenerator.execute(container); 
        // container.fillMap();
        oceanFiller.generate();
        // erosionGenerator.execute(container);
        temperatureGenerator.generate();
        rainfallGenerator.generate();
        factionGenerator.generate();
        // elevationModifier.execute(container);
        // riverGenerator.execute(container);
        // brookGenerator.execute(container);
        // lakeGenerator.execute(container);
        // drainageGenerator.execute(container);
        // biomeGenerator.execute(container);
        // container.fillMap();
    }
}
}