using enums.material;
using enums.plant.raw;

namespace enums.plant {
    public class PlantType {
        public string name;
        public int[] tileXY;
        public int material; 
        public string atlasName;
        public bool isTree;

        // public int[] temperatureBounds; // min and max temperature
        // public int[] rainfallBounds;  // min and max painfall
        // public readonly List<PlantLifeStage> lifeStages = new List<PlantLifeStage>();
        // // public readonly List<PlacingTagEnum> placingTags = new ArrayList<>();
        // public List<int> plantingStart; // months, when plant can be planted on farms
        // public bool destroyOnHarvest;

        public bool isPlant;
        public bool isSubstrate;

        public PlantType(RawPlantType rawType) {
            name = rawType.name;
            tileXY = rawType.tileXY;
            material = rawType.material != null 
                ? MaterialMap.get().id(rawType.material)
                : MaterialMap.GENERIC_PLANT;
            // temperatureBounds = rawType.temperatureBounds; // min and max temperature
            // rainfallBounds = rawType.rainfallBounds;  // min and max rainfall
            // plantingStart = rawType.plantingStart;
            // destroyOnHarvest = rawType.destroyOnHarvest;
        }

        public void setTypeFlags() {
            // isTree = lifeStages[0].treeForm != null;
            // isSubstrate = materialName == null;
            // isPlant = !isTree && !isSubstrate;
        }
    }
}
