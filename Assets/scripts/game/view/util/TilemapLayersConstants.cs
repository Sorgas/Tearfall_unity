namespace game.view.util {
    public class TilemapLayersConstants {
        public const int FLOOR_LAYER = 9; // lowest (far from camera)
        public const int WALL_LAYER = 8;
        // items are 'like' on layer 8.5, 7.5 when on ramp 
        // items are 'like' on layer 8.5 

        public const int SELECTION_LAYER = 0;
        public const float GRID_STEP = 0.1f;
    }
}