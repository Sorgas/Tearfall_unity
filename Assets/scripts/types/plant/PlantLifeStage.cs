using System;
using System.Collections.Generic;
using types.plant.raw;

namespace types.plant {
    public class PlantLifeStage {
        public string[] titlePrefixSuffix;
        public int stageLength;
        public string harvestProduct; // products differ between stages
        public List<int> treeForm; // is null for non-trees
        public int productDropRatio; // number of product items per 1 block
        public string color;

        public int stageEnd; // calculated for faster checking

        public PlantLifeStage(RawPlantLifeStage rawStage) {
            
            titlePrefixSuffix = rawStage.titlePrefixSuffix;
            stageLength = rawStage.stageLength;
            harvestProduct = rawStage.harvestProduct;
            color = rawStage.color;
            treeForm = rawStage.treeForm;
            productDropRatio = rawStage.productDropRatio;
        }
    }
}
