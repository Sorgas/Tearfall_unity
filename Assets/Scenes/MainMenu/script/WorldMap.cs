using UnityEngine;
using System.Collections;

namespace Assets.Scenes.MainMenu.script {
    public class WorldMap {
        public float[,] elevation;
        public float[,] drainage;
        public float[,] summerTemperature;
        public float[,] winterTemperature;
        public float[,] rainfall;
        public Vector2[,] rivers;
        public Vector2[,] brooks;
        public Vector2[,] debug;
        public int[,] biome;

        public int width;
        public int height;
        public long seed;

        public WorldMap(int xSize, int ySize) {
            this.width = xSize;
            this.height = ySize;
            elevation = new float[xSize, ySize];
            drainage = new float[xSize, ySize];
            summerTemperature = new float[xSize, ySize];
            winterTemperature = new float[xSize, ySize];
            rainfall = new float[xSize, ySize];
            biome = new int[width, height];

            rivers = new Vector2[xSize, ySize];
            brooks = new Vector2[xSize, ySize];
            debug = new Vector2[xSize, ySize];
        }
    }
}