using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.enums.plant.raw {
    class RawPlantType {
        public string name;
        public string title;
        public string materialName = "generic_plant"; // in null for substrates
        public string description;

        public int[] temperatureBounds = new int[2]; // min and max temperature
        public int[] rainfallBounds = new int[2];  // min and max painfall
        public List<RawPlantLifeStage> lifeStages = new List<RawPlantLifeStage>();
        public List<string> placingTags = new List<string>();
        // public HashSet<PlacingTagEnum> placingTagsSet = new HashSet<>();
        public List<int> plantingStart = new List<int>();
        public int[] atlasXY = new int[2];
        public bool destroyOnHarvest;
    }
}
