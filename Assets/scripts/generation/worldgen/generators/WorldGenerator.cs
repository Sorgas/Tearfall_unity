using UnityEngine.Tilemaps;

namespace Assets.scripts.generation.worldgen.generators {
    // Gets generation parameters from ui, launches generation, renders world to tilemap
    public abstract class WorldGenerator {
        public Tilemap tilemap;
        public TileBase tile;

        public abstract void generate(WorldGenConfig config, WorldGenContainer container);
    }
}