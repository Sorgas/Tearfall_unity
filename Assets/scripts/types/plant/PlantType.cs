using types.material;
using types.plant.raw;

namespace types.plant {
    public class PlantType {
        public string name;
        public string title;
        public int material;

        public float maxAge; // in days, plant dies after this
        // TODO use in hours maturity age and apply growth only when plant is lit
        public float maturityAge; // in days, plant becomes harvestable, last sprite applied, fertility can speed up growth

        public bool isTree; // false by default
        public bool destroyOnHarvest;
        // TODO add require light flag
        
        // render
        public int[] tileXY;
        public int tiles; // number of tiles in row. [n - 1] tiles are equally spread along growth period (growthStages), last tile is shown when plant is mature
        public string atlasName;
        public float[] growthStages; // [0; 1] based on number of tiles 

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
            maxAge = raw.maxAge;
            maturityAge = raw.maturityAge;
            isTree = raw.isTree;
            destroyOnHarvest = raw.destroyOnHarvest;
            tileXY = raw.tileXY;
            tiles = raw.tiles;
            atlasName = raw.atlasName;
            productItemType = raw.productItemType;
            productMaterial = raw.productMaterial;
            productCount = raw.productCount;
            harvestMonth = raw.harvestMonth;
            // lifeStages = rawType.lifeStages
            //     .Select(rawStage => new PlantLifeStage(rawStage))
            //     .ToList();
            // temperatureBounds = rawType.temperatureBounds; // min and max temperature
            // rainfallBounds = rawType.rainfallBounds;  // min and max rainfall
            // plantingStart = rawType.plantingStart;
            // destroyOnHarvest = rawType.destroyOnHarvest;
            countLifeStages(raw);
        }

        private void countLifeStages(RawPlantType raw) {
            growthStages = new float[raw.tiles];
            for (int i = 0; i < raw.tiles; i++) {
                growthStages[i] = (i + 1) / (float)tiles;
            }
        }

        public void setTypeFlags() {
            // isTree = lifeStages[0].treeForm != null;
            // isSubstrate = materialName == null;
            // isPlant = !isTree && !isSubstrate;
        }
    }
}