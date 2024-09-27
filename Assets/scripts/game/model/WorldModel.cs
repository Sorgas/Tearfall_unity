// contains WorldMap and Ecs infrastructure for world entities

using System.Collections.Generic;
using game.model.component;

namespace game.model {
    public class WorldModel {
        public string worldName;
        public WorldMap worldMap;
        public Dictionary<string, Faction> factions = new();
    }
}