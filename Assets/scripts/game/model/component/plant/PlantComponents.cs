using System.Collections.Generic;
using enums.plant;

namespace game.model.component.plant {

    public struct PlantComponent {
        public PlantType type;
        public PlantBlock block;
    }
    
    // stores tree blocks
    public struct TreeComponent {
        public List<PlantBlock> blocks;
    }
    
    public struct 
}