using System.Collections.Generic;
using types.plant;
using UnityEngine;

namespace game.model.component.plant {

    public struct PlantComponent {
        public PlantType type;
        public PlantBlock block;
        public float age; // in days
        public float growth; // [0;1] speed up by fertility
        public int currentStage;
    }

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
        public PlantUpdateType type; 

    }
    
    public struct PlantRemoveComponent {
        public bool leaveProduct;
    }

    public struct PlantUpdateComponent {
        public PlantUpdateType type;
    }

    // used both for visual and model updates
    public enum PlantUpdateType {
        GROW,
        NEW
        // TODO add damage, burn, snow, dry
    }
}
