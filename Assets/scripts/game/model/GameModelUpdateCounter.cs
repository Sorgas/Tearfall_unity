using System;

namespace game.model {
    public class GameModelUpdateCounter {
        public int lastUPS;
        private int updateCounter;
        private int second;

        public void update() {
            updateCounter++;
            if (DateTime.Now.TimeOfDay.Seconds != second) {
                second = DateTime.Now.TimeOfDay.Seconds;
                lastUPS = updateCounter;
                updateCounter = 0;
            }
        }
    }
}