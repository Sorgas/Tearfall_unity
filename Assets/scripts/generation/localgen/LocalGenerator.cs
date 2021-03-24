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
    public abstract class LocalGenerator {

        protected LocalGenerator() { }

        public abstract void generate();
    }
}
