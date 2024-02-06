using System;
using UnityEngine;

namespace game.model {
    // Counts how many update ticks should be performed during 1 engine update
    // Should be called once per update
    public class GameModelUpdateController {
        public const float UPDATE_TICK_DELTA = 1 / 30f; // ticks per second on normal game speed
        public bool paused = false;
        public int speed = 1; // [1; 3]
        private float remainingTime; // excess time after counting ticks
        private float previous = Time.realtimeSinceStartup; // time of previous call
        private float previousSpeed = 1;
        
        // returns ticks to be passed to game model
        public int getTicksForUpdate() {
            float timePassed = Time.deltaTime + remainingTime;
            remainingTime = timePassed % UPDATE_TICK_DELTA;
            return (int)Math.Floor(timePassed / UPDATE_TICK_DELTA);
        }

        public void setSpeed(int newSpeed) {
            Time.timeScale = newSpeed;
            speed = newSpeed;
        }

        public void togglePaused() {
            if (Time.timeScale != 0) {
                previousSpeed = Time.timeScale;
                Time.timeScale = 0;
            } else {
                Time.timeScale = previousSpeed;
            }
        }
    }
}