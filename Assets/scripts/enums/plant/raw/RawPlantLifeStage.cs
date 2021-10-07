using System;
using System.Collections.Generic;

namespace enums.plant.raw {
    class RawPlantLifeStage {
        public String[] titlePrefixSuffix;
        public int stageLength;
        public String harvestProduct;
        public String color = "0xffffffff"; // white is default
        public List<int> treeForm; // not null only for trees
        public int productDropRatio;
    }
}
