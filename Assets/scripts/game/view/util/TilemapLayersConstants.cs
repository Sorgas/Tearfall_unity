namespace game.view.util {
    // used to organize tiles and sprites inside one cell. 
    public static class TilemapLayersConstants {
        public const int SELECTION_LAYER = 0;
        
        public const int SUBSTRATE_WALL_LAYER = 9;
        public const int WALL_LAYER = 10;
        public const int UNIT_LAYER = 12; // also for items, etc
        public const int BUILDING_LAYER = 14; // also for plants
        public const int SUBSTRATE_RAMP_LAYER = 15;
        public const int ZONE_FLOOR_LAYER = 16;
        public const int SUBSTRATE_FLOOR_LAYER = 17;
        public const int FLOOR_LAYER = 18; // lowest (far from camera)

        public const float GRID_STEP = 0.05f;
    }
}