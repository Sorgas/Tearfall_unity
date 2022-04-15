using Leopotam.Ecs;

namespace game.model.system {
    public abstract class EcsRunIntervalSystem : IEcsRunSystem {
        private int counter;
        private int interval;

        protected EcsRunIntervalSystem(int interval) {
            this.interval = interval;
        }

        public abstract void runLogic();

        public void Run() {
            if (rollTimer()) runLogic();
        }
        
        private bool rollTimer() {
            counter++;
            if (counter >= interval) {
                counter = 0;
                return true;
            }
            return false;
        }
    }
}