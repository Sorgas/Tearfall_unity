
using System.Collections.Generic;

namespace game.model {
    public class World {
        public WorldModel worldModel = new();
        public Dictionary<string, LocalModel> localMapModels = new();
    }
}
