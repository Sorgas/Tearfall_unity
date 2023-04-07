using System;
using System.Collections.Generic;
using types.plant.raw;

namespace types.plant {
    public class PlantLifeStage {
        
        public string[] titlePrefixSuffix;
        public int stageLength;
        public List<int> treeForm; // is null for non-trees
        public string color;

        public int stageEnd; // calculated for faster checking

        public PlantLifeStage(RawPlantLifeStage rawStage) {
            
            titlePrefixSuffix = rawStage.titlePrefixSuffix;
            stageLength = rawStage.stageLength;
            color = rawStage.color;
            treeForm = rawStage.treeForm;
        }
    }
}
