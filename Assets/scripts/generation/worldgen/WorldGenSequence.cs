using generation.worldgen.generators;
using generation.worldgen.generators.drainage;
using generation.worldgen.generators.elevation;
using generation.worldgen.generators.temperature;

namespace generation.worldgen {
public class WorldGenSequence {
    private WorldNameGenerator nameGenerator = new();
    private WorldElevationGenerator elevationGenerator = new();
    private WorldOceanFiller oceanFiller = new();
    private WorldTemperatureGenerator temperatureGenerator = new();
    private RainfallGenerator rainfallGenerator = new();
    // private ErosionGenerator erosionGenerator;
    // private ElevationModifier elevationModifier;
    // private RiverGenerator riverGenerator;
    // private LakeGenerator lakeGenerator;
    // private BrookGenerator brookGenerator;
    // private DrainageGenerator drainageGenerator;
    // private BiomeGenerator biomeGenerator;
    // private CelestialBodiesGenerator celestialBodiesGenerator;

    public void run() {
        nameGenerator.generate();
        elevationGenerator.generate(); // generates elevation [0, 1]
        // celestialBodiesGenerator.execute(container); 
        // container.fillMap();
        oceanFiller.generate();
        // erosionGenerator.execute(container);
        temperatureGenerator.generate();
        rainfallGenerator.generate();
        // elevationModifier.execute(container);
        // riverGenerator.execute(container);
//        brookGenerator.execute(container);
//        lakeGenerator.execute(container);
//        drainageGenerator.execute(container);
//        biomeGenerator.execute(container);
        // container.fillMap();
    }
}
}