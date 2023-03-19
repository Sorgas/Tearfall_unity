using System.Collections.Generic;
using System.Linq;
using types.material;
using types.plant.raw;

namespace types.plant {
    public class PlantType {
        public string name;
        public string title;
        public int[] tileXY;
        public int material; 
        public string atlasName;
        public bool isTree;

        public int[] temperatureBounds; // min and max temperature
        public int[] rainfallBounds;  // min and max painfall
        public readonly List<PlantLifeStage> lifeStages = new();
        public readonly List<string> placingTags = new ();
        public List<int> plantingStart; // months, when plant can be planted on farms
        public bool destroyOnHarvest;

        public bool isPlant;
        public bool isSubstrate;
        public string productItem; // icon for ui 
        
        public PlantType(RawPlantType rawType) {
            name = rawType.name;
            title = rawType.title ?? name;
            tileXY = rawType.tileXY;
            material = rawType.material != null 
                ? MaterialMap.get().id(rawType.material)
                : MaterialMap.GENERIC_PLANT;
            productItem = rawType.productItem;
            lifeStages = rawType.lifeStages
                .Select(rawStage => new PlantLifeStage(rawStage))
                .ToList();
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
