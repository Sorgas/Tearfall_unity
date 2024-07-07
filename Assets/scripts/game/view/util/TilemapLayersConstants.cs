namespace game.view.util {
    // used to organize tiles and sprites inside one cell. 
    public static class TilemapLayersConstants {
        public const int SELECTION_LAYER = 0; // highest (close to camera)

        public const int SUBSTRATE_WALL_LAYER = 8;
        // public const int ORE_WALL_LAYER = 9;
        public const int WALL_LAYER = 10;
        public const int UNIT_LAYER = 10; // also for items, etc
        public const int BUILDING_LAYER = 13; // also for plants
        public const int SUBSTRATE_RAMP_LAYER = 13;
        public const int ORE_RAMP_LAYER = 14;
        public const int ROOM_LAYER = 15;
        public const int ZONE_LAYER = 16;
        public const int SUBSTRATE_FLOOR_LAYER = 17;
        // public const int ORE_FLOOR_LAYER = 18;
        public const int FLOOR_LAYER = 19; // lowest (far from camera)

        public const float GRID_STEP = 0.05f;
    }
}