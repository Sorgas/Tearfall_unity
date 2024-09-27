using System.Collections.Generic;
using System.Linq;
using game.model;
using game.model.container;
using util.geometry;

namespace generation.worldgen {
    public class WorldGenContainer {
        public string worldName;
        public float[,] elevation;
        public float[,] drainage;
        public float[,] summerTemperature;
        public float[,] winterTemperature;
        public float[,] rainfall;
        public IntVector2[,] rivers;
        public IntVector2[,] brooks;
        public IntVector2[,] debug;
        public int[,] biome;

        public int size;
        public Dictionary<string, FactionDescriptor> factions = new();
        
        // public Random random;

        public WorldGenContainer(WorldGenConfig config) {
            size = config.size;
            elevation = new float[size, size];
            drainage = new float[size, size];
            summerTemperature = new float[size, size];
            winterTemperature = new float[size, size];
            rainfall = new float[size, size];
            biome = new int[size, size];

            rivers = new IntVector2[size, size];
            brooks = new IntVector2[size, size];
            debug = new IntVector2[size, size];
        }

        public WorldModel createWorldModel() {
            WorldModel model = new();
            model.worldName = worldName;
            model.worldMap = createWorldMap();
            model.factions = factions.Values
                .Select(createFaction)
                .ToDictionary(faction => faction.name, faction => faction);
            return model;
        }
        
        // collect intermediate results into worldMap
        private WorldMap createWorldMap() {
            WorldMap worldMap = new(size);
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    worldMap.elevation[x, y] = elevation[x, y];
                    worldMap.summerTemperature[x, y] = summerTemperature[x, y];
                    worldMap.winterTemperature[x, y] = winterTemperature[x, y];
                    worldMap.rainfall[x, y] = rainfall[x, y];
                }
            }
            return worldMap;
        }

        private Faction createFaction(FactionDescriptor descriptor) {
            Faction faction = new Faction(descriptor.name);
            faction.relation = descriptor.relation;
            faction.strategy = descriptor.behaviour;
            return faction;
        }
    }
}