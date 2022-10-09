using System.Collections.Generic;
using generation.localgen.generators;

namespace generation.localgen {
    // holds and launches local generators
    // generators generate final data into GameModel and intermediate data to LocalGenContainer 
    public class LocalGenSequence {
        private LocalMapGenerator generator;
        public int progress = 0;
        public int maxProgress = 0;
        public string currentMessage;
        private List<LocalGenerator> generators = new();

        //private LocalRiverGenerator localRiverGenerator;
        //private LocalCaveGenerator localCaveGenerator;
        //private LocalPlantsGenerator localPlantsGenerator;
        //private LocalSubstrateGenerator localSubstrategenerator;
        //private LocalFaunaGenerator localFaunaGenerator;
        //private LocalItemsGenerator localItemsGenerator;
        //private LocalTemperatureGenerator localTemperatureGenerator;
        //private LocalSurfaceWaterPoolsGenerator localSurfaceWaterPoolsGenerator;
        //private LocalOresGenerator localOresGenerator;

        public LocalGenSequence(LocalMapGenerator generator) {
            this.generator = generator;
            // generate to localMap
            generators.Add(new LocalElevationGenerator(generator));
            generators.Add(new LocalStoneLayersGenerator(generator));
            generators.Add(new LocalRampFloorPlacer(generator));
            // generate to 
            generators.Add(new LocalBuildingGenerator(generator));
            generators.Add(new LocalUnitGenerator(generator));
            generators.Add(new LocalForestGenerator(generator));
            generators.Add(new LocalItemGenerator(generator));
            maxProgress = generators.Count - 1;
        }

        public LocalModel run() {
            generators.ForEach(generator => {
                currentMessage = generator.getMessage();
                generator.generate();
                progress++;
            });
            return generator.localGenContainer.model;
        }
    }
}