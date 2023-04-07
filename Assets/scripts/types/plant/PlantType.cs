using types.material;
using types.plant.raw;

namespace types.plant {
    public class PlantType {
        public string name;
        public string title;
        public int material;
        public float lifespan; // in days
        public bool isTree; // false by default
        public bool destroyOnHarvest;
        
        // render
        public int[] tileXY;
        public int tiles; // length of tiles row. tiles are equally spread along lifespan, last tile is shown when plant is ready for harvest.
        public string atlasName;

        // product
        public string productItemType; // products differ between stages
        public string productMaterial; // default is generic_plant
        public int productCount; // per block, scaled to plant health
        public float harvestPeriod; // when present, plant is harvestable in the end of lifespan
        public string harvestMonth; // when present, plant is harvestable in specified month
        
        // tree
        // public list<TreeForm> treeForms; 
        
        // public int[] temperatureBounds; // min and max temperature
        // public int[] rainfallBounds;  // min and max painfall
        // public readonly List<PlantLifeStage> lifeStages = new();
        // public readonly List<string> placingTags = new ();
        
        public PlantType(RawPlantType raw) {
            name = raw.name;
            title = raw.title ?? name;
            material = raw.material != null 
                ? MaterialMap.get().id(raw.material)
                : MaterialMap.GENERIC_PLANT;
            lifespan = raw.lifespan;
            isTree = raw.isTree;
            destroyOnHarvest = raw.destroyOnHarvest;
            tileXY = raw.tileXY;
            tiles = raw.tiles;
            atlasName = raw.atlasName;
            productItemType = raw.productItemType;
            productMaterial = raw.productMaterial;
            productCount = raw.productCount;
            harvestPeriod = raw.harvestPeriod;
            harvestMonth = raw.harvestMonth;
            // lifeStages = rawType.lifeStages
            //     .Select(rawStage => new PlantLifeStage(rawStage))
            //     .ToList();
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
