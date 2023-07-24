
using System.Collections.Generic;
using game.model.localmap;

namespace game.model {
    public class World {
        public string name;
        public WorldModel worldModel = new();
        public Dictionary<string, LocalModel> localMapModels = new();
    }
}
