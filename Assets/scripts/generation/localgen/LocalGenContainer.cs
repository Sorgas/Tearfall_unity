using System.Collections.Generic;
using game.model;
using game.model.localmap;
using UnityEngine;
using util.geometry;

namespace generation.localgen {
// Container for intermediate and final results of localMap generation.
// Creates and stores LocalModel during generation.
// LocalModel creates EcsWorld immediately, generators can create entities directly into it.
public class LocalGenContainer {
        public string mapName;
        public LocalModel model;
        public LocalMap map;

        // intermediate data
        public int localElevation;
        public float[,] heightsMap;
        public float[] monthlyTemperatures = new float[12];
        public List<IntVector3> waterSources = new();
        public List<IntVector3> waterTiles = new();

        // to generate
        public Dictionary<string, string> buildingsToAdd = new(); // buiding type to material
        public Dictionary<string, string[]> itemsToStore = new();
        
        public LocalGenContainer(string name) {
            mapName = name;
            LocalGenConfig config = GenerationState.get().localMapGenerator.localGenConfig;
            heightsMap = new float[config.areaSize, config.areaSize];
            World world = GameModel.get().world;
            localElevation = (int) (world.worldModel.worldMap.elevation[config.location.x, config.location.y] * config.localElevationMultiplier);
            Debug.Log("[LocalGenContainer]: area size " + config.areaSize + " localElevation: " + localElevation);
            model = new LocalModel();
            model.localMap = new LocalMap(config.areaSize, config.areaSize, localElevation + config.airLayersAboveGround, model);
            map = model.localMap;
        }
    }
}
