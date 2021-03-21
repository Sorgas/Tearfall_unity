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
    public class LocalElevationGenerator : LocalGenerator {

        public override void generate(LocalGenConfig config, LocalGenContainer container) {
            for (int x = 0; x < config.areaSize; x++) {
                for (int y = 0; y < config.areaSize; y++) {
                    container.heightsMap[x, y] = config.localElevation;
                }
            }
        }
    }
}
