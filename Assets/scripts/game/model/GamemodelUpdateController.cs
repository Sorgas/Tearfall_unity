using System;
using UnityEngine;

namespace game.model {
    // updates model with one of 3 speed settings. 
    public class GameModelUpdateController {
        public const float UPDATE_TICK_DELTA = 1 / 30f; // ticks per second on normal gamespeed
        public bool paused = false;
        public int speed = 1; // [1; 3]
        private readonly TimeIntervalCounter intervalCounter;

        private readonly GameModel model;
        private readonly GameSpeedController speed1 = new(3);
        private readonly GameSpeedController speed2 = new(2);
        private readonly GameSpeedController speed3 = new(1);
        private GameSpeedController currentSpeed;
        private float timeCounter = 0;
        private float remainingTime;

        public GameModelUpdateController(GameModel model) {
            this.model = model;
            currentSpeed = speed1;
            intervalCounter = new();
            intervalCounter.init();
        }

        public void update(float delta) {
            if (paused) return;

            // timeCounter += delta;
            // if (currentSpeed.update()) {
            //     model.update();
            //     timeCounter = 0;
            // }

            float timePassed = intervalCounter.get() * speed + remainingTime;
            int wholeTicks = (int)Math.Floor(timePassed / UPDATE_TICK_DELTA);
            remainingTime = timePassed % UPDATE_TICK_DELTA;
            Debug.Log(wholeTicks);
            model.update(wholeTicks);
        }

        public void setSpeed(int speed) {
            this.speed = speed;
            currentSpeed = speed switch {
                1 => speed1,
                2 => speed2,
                3 => speed3,
                _ => currentSpeed
            };
        }

        // returs true each [interval] updates
        private class GameSpeedController {
            public readonly int interval;
            private int counter;

            public GameSpeedController(int interval) => this.interval = interval;

            public bool update() {
                counter++;
                if (counter != interval) return false;
                counter = 0;
                return true;
            }
        }

        // counts intervals in seconds between calls of its get() method
        private class TimeIntervalCounter {
            private float previous;

            public void init() => previous = Time.realtimeSinceStartup;

            public float get() {
                float old = previous;
                previous = Time.realtimeSinceStartup;
                return previous - old;
            }

        }
    }
}