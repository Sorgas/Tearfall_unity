using System;

namespace game.model {
    // Counts updates for debug purposes
    public class GameModelUpdateCounter {
        public int lastUps;
        private int updateCounter;
        private int second;

        public void update(int updates) {
            updateCounter += updates;
            if (DateTime.Now.TimeOfDay.Seconds != second) {
                second = DateTime.Now.TimeOfDay.Seconds;
                lastUps = updateCounter;
                updateCounter = 0;
            }
        }
    }
}