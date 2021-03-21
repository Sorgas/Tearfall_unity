using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.geometry;

namespace Assets.scripts.generation.localgen {
    public class LocalGenContainer {
        public LocalGenConfig config;
        private World world;
        public LocalMap localMap;

        public int[,] heightsMap;
        public float[] monthlyTemperatures = new float[12];

        public List<IntVector3> waterSources = new List<IntVector3>();
        public List<IntVector3> waterTiles = new List<IntVector3>();

        public LocalGenContainer(LocalGenConfig config, World world) {
            this.config = config;
            this.world = world;
            heightsMap = new int[config.areaSize, config.areaSize];
            config.localElevation = (int)(world.worldMap.elevation[config.location.x, config.location.y] * config.worldToLocalElevationModifier);
        }

        public void createLocalMap() {
            localMap = new LocalMap(config.areaSize, config.areaSize, config.localElevation + config.airLayersAboveGround);
        }
    }
}
