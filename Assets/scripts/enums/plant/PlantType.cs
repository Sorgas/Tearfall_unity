using System;
using System.Collections.Generic;
using enums.plant.raw;

namespace enums.plant {
    class PlantType {
        public String name;
        public String title;
        public String materialName; // is null for substrates
        public String description;
        public String atlasName;

        public int[] temperatureBounds; // min and max temperature
        public int[] rainfallBounds;  // min and max painfall
        public readonly List<PlantLifeStage> lifeStages = new List<PlantLifeStage>();
        // public readonly List<PlacingTagEnum> placingTags = new ArrayList<>();
        public List<int> plantingStart; // months, when plant can be planted on farms
        public int[] atlasXY;
        public bool destroyOnHarvest;

        public bool isPlant;
        public bool isTree;
        public bool isSubstrate;

        public PlantType(RawPlantType rawType) {
            name = rawType.name;
            title = rawType.title;
            materialName = rawType.materialName;
            description = rawType.description;
            temperatureBounds = rawType.temperatureBounds; // min and max temperature
            rainfallBounds = rawType.rainfallBounds;  // min and max rainfall
            plantingStart = rawType.plantingStart;
            atlasXY = rawType.atlasXY;
            destroyOnHarvest = rawType.destroyOnHarvest;
        }

        public void setTypeFlags() {
            isTree = lifeStages[0].treeForm != null;
            isSubstrate = materialName == null;
            isPlant = !isTree && !isSubstrate;
        }

        public int getMaxAge() {
            return lifeStages[lifeStages.Count - 1].stageEnd;
        }

        public String toString() {
            return title;
        }
    }
}
