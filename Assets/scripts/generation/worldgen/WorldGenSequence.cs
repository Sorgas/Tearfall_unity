using game.model;
using generation.worldgen.generators.elevation;

namespace generation.worldgen {
    public class WorldGenSequence {
        
        private WorldElevationGenerator elevationGenerator;
        // private OceanFiller oceanFiller;
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

        public WorldGenSequence() {
            elevationGenerator = new WorldElevationGenerator();
            // oceanFiller = new WorldOceanFiller();
            // riverGenerator = new RiverGenerator();
            // brookGenerator = new BrookGenerator();
            // temperatureGenerator = new TemperatureGenerator();
            // rainfallGenerator = new RainfallGenerator();
            // erosionGenerator = new ErosionGenerator();
            // elevationModifier = new ElevationModifier();
            // lakeGenerator = new LakeGenerator();
            // drainageGenerator = new DrainageGenerator();
            // biomeGenerator = new BiomeGenerator();
            // celestialBodiesGenerator = new CelestialBodiesGenerator();
        }

        public void run() {
            elevationGenerator.generate(); // generates elevation [0, 1]
            // celestialBodiesGenerator.execute(container); 
            // container.fillMap();
            // oceanFiller.execute(container);
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