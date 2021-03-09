using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.enums.plant.raw {
    class RawPlantLifeStage {
        public String[] titlePrefixSuffix;
        public int stageLength;
        public String harvestProduct;
        public String color = "0xffffffff"; // white is default
        public List<int> treeForm; // not null only for trees
        public int productDropRatio;
    }
}
