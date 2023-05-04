using System;
using types.material;

namespace types.plant {
    /**
     * Plants have several periods:
     *      Life span - plants die after reaching maxAge. Progress independent from conditions.
     *      Growth period - Plant is growing from young to adult plant(maturityAge). Progress depends on conditions.
     *      Product growth waiting - Plant is to young to grow products. Defined by productGrowthStart relatively to Growth period. Progress independent from conditions.
     *      Product growth period - Plant is growing its fruits. From productGrowthStart to productGrowthTime. Progress depends on conditions.
     *      Product keeping period - Plant products are ready to be harvested and will stay for productKeepTime. Progress independent from conditions. 
     */
    public class PlantType : RawPlantType {
        public int materialId; // used for tree logs, plants burning
        public float[] growthStages; // [0+; maturityAge * 2], stores growth value when each stage ends
        public float productGrowthStartAbsolute; // calculated from productGrowthStart

        public PlantType() { }
        
        public void init() {
            materialId = material != null
                ? MaterialMap.get().id(material)
                : MaterialMap.GENERIC_PLANT;
            countLifeStages();
            if (productItemType != null) {
                productGrowthStartAbsolute = maturityAge * productGrowthStart;
                if (productGrowthTime <= 0) { // if growth time is not set, product will grow on maturityAge
                    productGrowthTime = maturityAge - productGrowthStartAbsolute;
                }
            }
        }

        // array stores growth value of stage end. last stage has 2 and never ends,  
        private void countLifeStages() {
            growthStages = new float[tiles];
            if (tiles > 1) {
                float stageLength = maturityAge / (tiles - 1);
                for (int i = 0; i < tiles - 1; i++) {
                    growthStages[i] = stageLength * (i + 1);
                }
            }
            growthStages[tiles - 1] = maturityAge * 2;
        }

        public int getStageByAge(float age) {
            if (tiles == 1) return 0;
            if (age > maturityAge) return tiles - 1;
            return (int) Math.Floor(age / (maturityAge / (tiles - 1)));
        }
    }

    [Serializable]
    // to read from json
    public class RawPlantType {
        public string name;
        public string title;
        public string description;
        public string material = "generic_plant"; // is null for substrates

        // lifespan
        // TODO use in hours maturity age and apply growth only when plant is lit
        public float maxAge; // in days, plant dies after this
        public float maturityAge; // in days, defines growing period
        public float productGrowthStart; // [0; 1] relative to growing period, default 0
        public float productGrowthTime; // in days, defines product growing period, default maturityAge
        public float productKeepTime; // in days, defines product harvestable period, default forever

        // render
        public int[] tileXY = new int[2];
        // [n - 1] tiles are equally spread along growth period (growthStages), last tile is shown when plant is mature
        public int tiles;
        public string atlasName; // same as json file by default

        // product
        public string productItemType; // should be present for other fields to work
        public string productMaterial = "generic_plant";
        public int productCount; // per block, scaled to plant health
        public string harvestMonth; // when present, plant is harvestable in specified month

        // flags
        // TODO add require light flag
        public bool isTree; // false by default
        public bool destroyOnHarvest; // false by default

        public RawPlantType() {
            productGrowthStart = 0; // plant will grow products from planting
            productKeepTime = -1; // keep products forever
        }
    }
}