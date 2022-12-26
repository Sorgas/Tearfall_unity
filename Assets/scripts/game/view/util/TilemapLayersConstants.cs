namespace game.view.util {
    // used to organize tiles and sprites inside one cell. 
    public static class TilemapLayersConstants {
        public const int SUBSTRATE_WALL_LAYER = 4;
        public const int WALL_LAYER = 5;
        
        public const int UNIT_LAYER = 6; // also for items, etc
        public const int BUILDING_LAYER = 7; // also for plants
        
        public const int SUBSTRATE_FLOOR_LAYER = 8;
        public const int FLOOR_LAYER = 9; // lowest (far from camera)
        // items are 'like' on layer 8.5, 7.5 when on ramp 

        public const int SELECTION_LAYER = 0;
        public const float GRID_STEP = 0.1f;
        
    }
}