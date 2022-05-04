using System.Collections.Generic;

namespace enums.plant.raw {
    public class RawPlantType {
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
