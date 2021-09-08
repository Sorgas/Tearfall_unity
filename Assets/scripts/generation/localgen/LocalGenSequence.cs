using Assets.scripts.game.model.localmap;
using Assets.scripts.generation.localgen.generators;
using Tearfall_unity.Assets.scripts.generation.localgen.generators;

namespace Assets.scripts.generation.localgen {
    public class LocalGenSequence {
        private LocalElevationGenerator localElevationGenerator;
        private LocalStoneLayersGenerator localStoneLayersGenerator;
        //private LocalRiverGenerator localRiverGenerator;
        //private LocalCaveGenerator localCaveGenerator;
        private LocalRampFloorPlacer localRampFloorPlacer;
        //private LocalPlantsGenerator localPlantsGenerator;
        //private LocalForestGenerator localForestGenerator;
        //private LocalSubstrateGenerator localSubstrategenerator;
        //private LocalFaunaGenerator localFaunaGenerator;
        //private LocalBuildingGenerator localBuildingGenerator;
        //private LocalItemsGenerator localItemsGenerator;
        //private LocalTemperatureGenerator localTemperatureGenerator;
        //private LocalSurfaceWaterPoolsGenerator localSurfaceWaterPoolsGenerator;
        //private LocalOresGenerator localOresGenerator;
        private LocalUnitGenerator localUnitGenerator;

        public LocalGenSequence() {
            localElevationGenerator = new LocalElevationGenerator();
            localStoneLayersGenerator = new LocalStoneLayersGenerator();
            //localOresGenerator = new
            //localCaveGenerator = new LocalCaveGenerator(localGenContainer);
            localRampFloorPlacer = new LocalRampFloorPlacer();
            //localTemperatureGenerator = new LocalTemperatureGenerator(localGenContainer);
            //localFaunaGenerator = new LocalFaunaGenerator(localGenContainer);
            //localBuildingGenerator = new LocalBuildingGenerator(localGenContainer);
            //localItemsGenerator = new LocalItemsGenerator(localGenContainer);
            //localPlantsGenerator = new LocalPlantsGenerator(localGenContainer);
            //localForestGenerator = new LocalForestGenerator(localGenContainer);
            //localSubstrategenerator = new LocalSubstrateGenerator(localGenContainer);
            //localSurfaceWaterPoolsGenerator = new LocalSurfaceWaterPoolsGenerator(localGenContainer);
            //localRiverGenerator = new LocalRiverGenerator(localGenContainer);
            localUnitGenerator = new LocalUnitGenerator();
        }

        public LocalMap run() {
            //landscape
            localElevationGenerator.generate();
            //creates heights map
            localStoneLayersGenerator.generate(); //fills localmap with blocks by heightsmap
            //localCaveGenerator.execute(); //digs caves
            //                              //water
            //                              //        localRiverGenerator.execute(); // carves river beds
            //localSurfaceWaterPoolsGenerator.execute(); // digs ponds
            localRampFloorPlacer.generate(); // places floors and ramps upon all top blocks
            //                                   //plants
            //localTemperatureGenerator.execute(); // generates year temperature cycle
            //localForestGenerator.execute(); // places trees
            //localPlantsGenerator.execute(); // places plants
            //localSubstrategenerator.execute(); // places substrates
            //                                   //creatures
            //localFaunaGenerator.execute(); // places animals
            //                               //buildings
            //localBuildingGenerator.execute();
            localUnitGenerator.generate();
            //localItemsGenerator.execute(); // places item

            return GenerationState.get().localGenContainer.localMap;
        }
    }
}