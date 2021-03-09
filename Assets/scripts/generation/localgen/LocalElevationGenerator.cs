using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.mainMenu;
using Assets.scripts.util.geometry;

namespace Assets.scripts.generation.localgen {
    class LocalElevationGenerator : LocalGenerator {
        
        public LocalElevationGenerator(LocalMap map, WorldMap worldMap, IntVector2 position, LocalGenConfig config) : base(map, worldMap, position, config) {
        }

        public override void generate() {
            
        }
    }
}
