using System;
using UnityEngine;

namespace game.model {
    // On each update counts real time and converts it to ticks to be calculated by model
    // Has game speed setting
    public class GameModelUpdateController {
        public const float UPDATE_TICK_DELTA = 1 / 30f; // ticks per second on normal game speed
        public bool paused = false;
        public int speed = 1; // [1; 3]
        private float remainingTime; // excess time after counting ticks
        private float previous = Time.realtimeSinceStartup; // time of previous call

        // returns ticks to be passed to game model
        public int getTicksForUpdate() {
            if (paused) {
                previous = Time.realtimeSinceStartup; // to prevent time spent in pause be counted after unpausing
                return 0;
            }
            float timePassed = getRealTimeDelta() * speed + remainingTime;
            remainingTime = timePassed % UPDATE_TICK_DELTA;
            return (int)Math.Floor(timePassed / UPDATE_TICK_DELTA);
        }

        public void setSpeed(int speed) => this.speed = speed;
        
        // returns seconds passed since previous call of this method
        private float getRealTimeDelta() {
            float old = previous;
            previous = Time.realtimeSinceStartup;
            return previous - old;
        }
    }
}