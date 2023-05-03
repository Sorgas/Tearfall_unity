using System.Collections.Generic;
using types.plant;
using UnityEngine;

namespace game.model.component.plant {

    public struct PlantComponent {
        public PlantType type;
        public PlantBlock block;
    }

    // plant counts age in any conditions and dies on max age (see PlantAgeSystem)
    public struct PlantAgeComponent {
        public float age; // in days
        public float maxAge;
    }
    
    // plant counts growth in comfort conditions, changes sprites, and becomes harvestable on max growth. (see PlantGrowthSystem)
    public struct PlantGrowthComponent {
        public float growth; // absolute time,  by fertility
        public float maturityAge; // max growth
        public int currentStage; // index in type.growthStages
        public float nextStage; // value from type.growthStages
    }

    // present on plants which give products and start glowing products not from life start
    public struct PlantProductGrowthWaitingComponent {
        public float productGrowthStartAbsolute;
    }
    
    public struct PlantProductGrowthComponent {
        public float growth;
        public float growthEnd;
    }
    
    public struct PlantHarvestableComponent {
    }

    public struct PlantHarvestKeepComponent {
        public float productKeepTime; // in days, harvest is destroyed after this
        public float harvestTime; 
    }
    
    public struct PlantHarvestedComponent { }
    
    // stores tree blocks
    public struct TreeComponent {
        public List<PlantBlock> blocks;
    }

    public struct PlantVisualComponent {
        public SpriteRenderer spriteRenderer;
        public GameObject go;
        public int tileNumber;
    }

    // added when something happens with plant
    public struct PlantVisualUpdateComponent {
        public List<PlantUpdateType> updates;

        public void add(PlantUpdateType type) {
            if (updates == null) updates = new();
            updates.Add(type);
        }
    }
    
    public struct PlantUpdateComponent {
        public PlantUpdateType type;
    }

    // used both for visual and model updates
    public enum PlantUpdateType {
        STAGE_CHANGE, // plant changes life stage
        NEW, // plant is new 
        GROWTH_COMPLETE, // plant become fully grown
        HARVEST_TIMEOUT, // harvest keep time elapsed
        HARVEST_UNIT, // plant harvested by unit
        HARVEST_READY, // product has grown
        REMOVE,
        // TODO add damage, burn, snow, dry
    }
}
