using System;
using UnityEngine;

namespace game.model {
    // Counts how many update ticks should be performed during 1 engine update
    // Should be called once per update
    public class GameModelUpdateController {
        public const float UPDATE_TICK_DELTA = 1f / GlobalSettings.UPDATES_PER_SECOND; // ticks per second on normal game speed
        private float remainingTime; // excess time after counting ticks
        
        // returns ticks to be passed to game model
        public int getTicksForUpdate() {
            float timePassed = Time.deltaTime + remainingTime;
            remainingTime = timePassed % UPDATE_TICK_DELTA;
            return (int)Math.Floor(timePassed / UPDATE_TICK_DELTA);
        }
    }
}