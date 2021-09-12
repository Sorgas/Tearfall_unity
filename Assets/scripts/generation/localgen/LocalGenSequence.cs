using System.Collections.Generic;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation.localgen.generators;
using Tearfall_unity.Assets.scripts.generation.localgen.generators;

namespace Assets.scripts.generation.localgen {
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