using System;
using System.Collections.Generic;

namespace types.plant.raw {
    
    [Serializable]
    public class RawPlantType {
        public string name;
        public string title;
        public string description;
        public string material = "generic_plant"; // is null for substrates
        public float lifespan; // in days
        public bool isTree; // false by default
        public bool destroyOnHarvest; // false by default

        // render
        public int[] tileXY = new int[2];
        public int tiles; // length of tiles row. tiles are equally spread along lifespan, last tile is shown when plant is ready for harvest.
        public string atlasName; // same as json file by default

        // product
        public string productItemType; // products differ between stages
        public string productMaterial; // generic_plant by default
        public int productCount; // per block, scaled to plant health
        public float harvestPeriod; // when present, plant is harvestable in the end of lifespan
        public string harvestMonth; // when present, plant is harvestable in specified month
        
        // public List<RawPlantLifeStage> lifeStages = new List<RawPlantLifeStage>();
        // public List<string> placingTags = new List<string>();
        // // "placingTags": ["water_near", "water_far", "soil_stone"],
        // // public HashSet<PlacingTagEnum> placingTagsSet = new HashSet<>();
        // public List<int> plantingStart = new List<int>();
        //
        // public int[] temperatureBounds = new int[2]; // min and max temperature
        // public int[] rainfallBounds = new int[2];  // min and max painfall
        // "temperatureBounds": ["-40", "40"],
        // "rainfallBounds": ["0", "100"],
    }
}