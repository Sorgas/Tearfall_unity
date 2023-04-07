using System.Collections.Generic;
using types.plant;
using UnityEngine;

namespace game.model.component.plant {

    public struct PlantComponent {
        public PlantType type;
        public PlantBlock block;
        public int age;
        public int growth; // 
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
}