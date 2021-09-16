using Leopotam.Ecs;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.model.system.unit {
    public class TaskSystem : IEcsRunSystem {
        public EcsFilter<TaskComponent>.Exclude<MovementTargetComponent> filter;
        private const int FAIL = -1; // action cannot be performed, fail task
        private const int PERFORM = 0; // action ready for performing
        private const int MOVE = 1; // movement required
        private const int SUBACTION = 2; // action condition created subaction
        

        public void Run() {
            foreach (int i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                ref TaskComponent task = ref filter.Get1(i);
                int checkResult = checkAction(task.getNextAction(), unit);
                switch(checkResult) {
                    case FAIL:
                    break;
                    case PERFORM:
                        unit.Replace<MovementTargetComponent>(new MovementTargetComponent()
                        {target = task.getNextAction().target.getPosition()});
                    break;
                    case MOVE: // set target for movement
                        unit.Replace<MovementTargetComponent>(new MovementTargetComponent()
                        {target = task.getNextAction().target.getPosition()});
                    break;
                    case SUBACTION:
                    break;
                }
            }
        }

        // checks if unit can perform action
        private int checkAction(Action action, EcsEntity unit) {
            return PERFORM;
        }
    }
}