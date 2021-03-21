using UnityEngine;
using System.Collections;

namespace Assets.scripts.mainMenu {
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

        public int size;
        public long seed;

        public WorldMap(int size) {
            this.size = size;
            elevation = new float[size, size];
            drainage = new float[size, size];
            summerTemperature = new float[size, size];
            winterTemperature = new float[size, size];
            rainfall = new float[size, size];
            biome = new int[size, size];
            rivers = new Vector2[size, size];
            brooks = new Vector2[size, size];
            debug = new Vector2[size, size];
        }
    }
}