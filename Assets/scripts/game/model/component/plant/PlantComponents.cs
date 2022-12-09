using System.Collections.Generic;
using enums.plant;
using UnityEngine;

namespace game.model.component.plant {

    public struct PlantComponent {
        public PlantType type;
        public PlantBlock block;
    }
    
    // stores tree blocks
    public struct TreeComponent {
        public List<PlantBlock> blocks;
    }

    public struct PlantVisualComponent {
        public SpriteRenderer spriteRenderer;
        public GameObject go;
    }

    public struct PlantRemoveComponent {
        public bool leaveProduct;
    }

    // grass covers ground and slowly spreads to adjacent ground
    public struct PlantGrassComponent {
    
    }
}