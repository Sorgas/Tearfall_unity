using System.Collections.Generic;
using generation.localgen.generators;

namespace generation.localgen {
    // holds and launches local generators
    // generators generate final data into GameModel and intermediate data to LocalGenContainer 
    public class LocalGenSequence {
        public int progress = 0;
        public int maxProgress = 0;
        public string currentMessage;
        private List<LocalGenerator> generators = new List<LocalGenerator>();

        //private LocalRiverGenerator localRiverGenerator;
        //private LocalCaveGenerator localCaveGenerator;
        //private LocalPlantsGenerator localPlantsGenerator;
        //private LocalForestGenerator localForestGenerator;
        //private LocalSubstrateGenerator localSubstrategenerator;
        //private LocalFaunaGenerator localFaunaGenerator;
        //private LocalBuildingGenerator localBuildingGenerator;
        //private LocalItemsGenerator localItemsGenerator;
        //private LocalTemperatureGenerator localTemperatureGenerator;
        //private LocalSurfaceWaterPoolsGenerator localSurfaceWaterPoolsGenerator;
        //private LocalOresGenerator localOresGenerator;

        public LocalGenSequence() {
            generators.Add(new LocalElevationGenerator());
            generators.Add(new LocalStoneLayersGenerator());
            generators.Add(new LocalRampFloorPlacer());
            generators.Add(new LocalUnitGenerator());
            generators.Add(new LocalForestGenerator());
            generators.Add(new LocalItemGenerator());
            maxProgress = generators.Count - 1;
        }

        public void run() {
            generators.ForEach(generator => {
                currentMessage = generator.getMessage();
                generator.generate();
                progress++;
            });
            // return GenerationState.get().localGenContainer.localMap;
        }
    }
}