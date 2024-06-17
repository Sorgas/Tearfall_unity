using generation.worldgen.generators;
using generation.worldgen.generators.drainage;
using generation.worldgen.generators.elevation;

namespace generation.worldgen {
public class WorldGenSequence {
    private WorldNameGenerator nameGenerator = new();
    private WorldElevationGenerator elevationGenerator = new();
    private WorldOceanFiller oceanFiller = new();
    // private TemperatureGenerator temperatureGenerator;
    // private RainfallGenerator rainfallGenerator;
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
        // temperatureGenerator.execute(container);
        // rainfallGenerator.execute(container);
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