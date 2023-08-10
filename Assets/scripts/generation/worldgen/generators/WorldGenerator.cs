using UnityEngine;

namespace generation.worldgen.generators {
    // Gets generation parameters from ui, launches generation, renders world to tilemap
    public abstract class WorldGenerator {
        protected WorldGenConfig config;
        protected WorldGenContainer container;
        // public Tilemap tilemap;
        // public TileBase tile;
        protected string name = "WorldGenerator";
        protected bool debug = false;
        
        protected WorldGenerator() {
            config = GenerationState.get().worldGenConfig;
            container = GenerationState.get().worldGenContainer;
        }

        public abstract void generate();

        protected void log(string message) {
            if(debug) Debug.Log($"[{name}]: {message}");
        }
    }
}