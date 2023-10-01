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
            if (product != null) {
                productGrowthStartAbsolute = maturityAge * productGrowthStart;
                if (productGrowthTime <= 0) { // if growth time is not set, product will grow on maturityAge
                    productGrowthTime = maturityAge - productGrowthStartAbsolute;
                }
            }
        }

        // array stores growth value of stage end. last stage has 2 and never ends,  
        private void countLifeStages() {
            growthStages = new float[tileCount];
            if (tileCount > 1) {
                float stageLength = maturityAge / (tileCount - 1);
                for (int i = 0; i < tileCount - 1; i++) {
                    growthStages[i] = stageLength * (i + 1);
                }
            }
            growthStages[tileCount - 1] = maturityAge * 2;
        }

        public int getStageByAge(float age) {
            if (tileCount == 1) return 0;
            if (age > maturityAge) return tileCount - 1;
            return (int) Math.Floor(age / (maturityAge / (tileCount - 1)));
        }
    }

    [Serializable]
    public class RawPlantType {
        public string name;
        public string title;
        public string description;
        public string material = "generic_plant"; // is null for substrates

        // TODO use in hours maturity age and apply growth only when plant is lit
        public float maxAge; // in days, plant dies after this
        public float maturityAge; // in days, defines growing period
        public float productGrowthStart = 0; // [0; 1] relative to growing period, default 0 to start immediately
        public float productGrowthTime; // in days, defines product growing period, default maturityAge
        public float productKeepTime = -1; // in days, defines product harvestable period, default forever

        // render. [n - 1] tiles are equally spread along growth period (growthStages), last tile is for adult plant
        public int[] tileXY = new int[2];
        public int tileCount = 1; // at least one tile
        public string atlasName = ""; // same as json file by default
        public int atlasSize = 64; // TODO switch to 32
        
        // flags
        // TODO add require light flag
        public bool isTree; // false by default
        public bool destroyOnHarvest; // destroy plant on first harvest

        public RawPlantProduct product;
        
        public RawPlantType() { }
    }

public class RawPlantProduct {
    public string productItemType; // should be present for other fields to work
    public string productMaterial = "generic_plant";
    public int productCount; // per block, scaled to plant health
    public string productOrigin;
}
}