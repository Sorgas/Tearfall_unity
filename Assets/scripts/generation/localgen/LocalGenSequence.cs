using System.Collections.Generic;
using game.model.localmap;
using generation.localgen.generators;

namespace generation.localgen {
    // holds and launches local generators
    // generators generate final data into GameModel and intermediate data to LocalGenContainer 
    public class LocalGenSequence {
        private LocalMapGenerator generator;
        public int progress;
        public int maxProgress;
        public string currentMessage;
        private List<LocalGenerator> generators = new();

        //private LocalRiverGenerator localRiverGenerator;
        //private LocalCaveGenerator localCaveGenerator;
        //private LocalPlantsGenerator localPlantsGenerator;
        //private LocalFaunaGenerator localFaunaGenerator;
        //private LocalItemsGenerator localItemsGenerator;
        //private LocalTemperatureGenerator localTemperatureGenerator;
        //private LocalSurfaceWaterPoolsGenerator localSurfaceWaterPoolsGenerator;
        //private LocalOresGenerator localOresGenerator;

        public LocalGenSequence(LocalMapGenerator generator) {
            this.generator = generator;
            generators.Add(new LocalElevationGenerator(generator));
            generators.Add(new LocalStoneLayersGenerator(generator));
            generators.Add(new LocalRampFloorPlacer(generator));
            generators.Add(new LocalSubstrateGenerator(generator));
            generators.Add(new LocalBuildingGenerator(generator));
            generators.Add(new LocalUnitGenerator(generator));
            generators.Add(new LocalForestGenerator(generator));
            generators.Add(new LocalPlantGenerator(generator));
            generators.Add(new LocalItemGenerator(generator));
        }

        public LocalModel run() {
            progress = 0;
            maxProgress = generators.Count;
            generators.ForEach(generator => {
                currentMessage = generator.getMessage();
                generator.generate();
                progress++;
            });
            return generator.localGenContainer.model;
        }
    }
}