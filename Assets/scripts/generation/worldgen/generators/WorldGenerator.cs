using UnityEngine.Tilemaps;

namespace Assets.scripts.generation.worldgen.generators {
    // Gets generation parameters from ui, launches generation, renders world to tilemap
    public abstract class WorldGenerator {
        protected WorldGenConfig config;
        protected WorldGenContainer container;
        // public Tilemap tilemap;
        // public TileBase tile;

        protected WorldGenerator() {
            config = GenerationState.get().worldGenConfig;
            container = GenerationState.get().worldGenContainer;
        }

        public abstract void generate();
    }
}