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
    abstract class LocalGenerator {
        protected LocalMap map;
        protected WorldMap worldMap;
        protected IntVector2 position;
        protected LocalGenConfig config;

        protected LocalGenerator(LocalMap map, WorldMap worldMap, IntVector2 position, LocalGenConfig config) {
            this.map = map;
            this.worldMap = worldMap;
            this.position = position;
            this.config = config;
        }

        public abstract void generate();
    }
}
