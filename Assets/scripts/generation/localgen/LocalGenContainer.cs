using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.geometry;
using UnityEngine;

namespace Assets.scripts.generation.localgen {
    public class LocalGenContainer {
        private World world;
        public LocalMap localMap;
        // public List<

        public float[,] heightsMap;
        public float[] monthlyTemperatures = new float[12];

        public List<IntVector3> waterSources = new List<IntVector3>();
        public List<IntVector3> waterTiles = new List<IntVector3>();

        public LocalGenContainer() {
            LocalGenConfig config = GenerationState.get().localGenConfig;
            this.world = GenerationState.get().world;
            heightsMap = new float[config.areaSize, config.areaSize];
            config.localElevation = (int)(world.worldMap.elevation[config.location.x, config.location.y] * config.localElevation);
            Debug.Log("localGenContainer: area size" + config.areaSize);
            localMap = new LocalMap(config.areaSize, config.areaSize, config.localElevation + config.airLayersAboveGround);
        }
    }
}
