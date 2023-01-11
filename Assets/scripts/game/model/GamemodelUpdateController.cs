

// updates model with one of 3 speed settings. 
namespace game.model {
    public class GameModelUpdateController {
        public const float updateTickDelta = 1/90f; // ticks per second on max gamespeed
        public bool paused;
        public int speed;

        private GameModel model;
        private GameSpeedController speed1 = new(3);
        private GameSpeedController speed2 = new(2);
        private GameSpeedController speed3 = new(1);
        private GameSpeedController currentSpeed;
        private float timeCounter = 0;

        public GameModelUpdateController(GameModel model) {
            this.model = model;
            currentSpeed = speed1;
        }

        public void update(float delta) {
            if (paused) return;
            timeCounter += delta;
            if (currentSpeed.update()) {
                model.update();
                timeCounter = 0;
            }
        }

        public void setSpeed(int speed) {
            if (speed == 1) {
                currentSpeed = speed1;
            } else if (speed == 2) {
                currentSpeed = speed2;
            } else if (speed == 3) {
                currentSpeed = speed3;
            }
        }

        private class GameSpeedController {
            public readonly int speed;
            private int counter = 0;

            public GameSpeedController(int speed) {
                this.speed = speed;
            }

            public bool update() {
                counter++;
                if (counter == speed) {
                    counter = 0;
                    return true;
                }
                return false;
            }
        }

        // time per tick
        public float getCurrentSpeed() {
            return currentSpeed.speed * updateTickDelta;
        }
    }
}