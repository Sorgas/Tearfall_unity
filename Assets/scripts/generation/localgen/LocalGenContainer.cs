using System.Collections.Generic;
using game.model;
using game.model.localmap;
using UnityEngine;
using util.geometry;

namespace generation.localgen {
    public class LocalGenContainer {

        public float[,] heightsMap;
        public float[] monthlyTemperatures = new float[12];

        public List<IntVector3> waterSources = new List<IntVector3>();
        public List<IntVector3> waterTiles = new List<IntVector3>();

        public LocalGenContainer() {
            LocalGenConfig config = GenerationState.get().localGenConfig;
            World world = GameModel.get().world;
            heightsMap = new float[config.areaSize, config.areaSize];
            config.localElevation = (int)(world.worldMap.elevation[config.location.x, config.location.y] * config.localElevation);
            Debug.Log("localGenContainer: area size" + config.areaSize);
            world.localMap = new LocalMap(config.areaSize, config.areaSize, config.localElevation + config.airLayersAboveGround);
        }
    }
}
