using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.mainMenu;

namespace Assets.scripts.game.model {
    public class World {
        public WorldMap worldMap;
        // TODO gods, celestial bodies, factions
        public World(WorldMap worldMap) {
            this.worldMap = worldMap;
        }
    }
}
