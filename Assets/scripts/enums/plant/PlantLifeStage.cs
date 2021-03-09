using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.enums.plant.raw;

namespace Assets.scripts.enums.plant {
    class PlantLifeStage {
        public String[] titlePrefixSuffix;
        public int stageLength;
        public String harvestProduct; // products differ between stages
        public List<int> treeForm; // is null for non-trees
        public int productDropRatio; // number of product items per 1 block
        public String color;

        public int stageEnd; // calculated for faster checking

        public PlantLifeStage(RawPlantLifeStage rawStage) {
            titlePrefixSuffix = rawStage.titlePrefixSuffix;
            stageLength = rawStage.stageLength;
            color = rawStage.color;
            treeForm = rawStage.treeForm;
            productDropRatio = rawStage.productDropRatio;
        }
    }
}
