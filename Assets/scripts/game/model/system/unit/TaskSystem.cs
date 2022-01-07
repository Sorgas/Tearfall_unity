using game.model.component;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using static game.model.component.task.TaskComponents;

namespace game.model.system.unit {
    // performs actions for units with tasks
    public class TaskSystem : IEcsRunSystem {
        public EcsFilter<UnitComponent, TaskComponent>.Exclude<MovementTargetComponent> filter;
        private const int FAIL = -1; // action cannot be performed, fail task
        private const int PERFORM = 0; // action ready for performing
        private const int MOVE = 1; // movement required
        private const int SUBACTION = 2; // action condition created subaction

        public void Run() {
            foreach (int i in filter) {
                Debug.Log("handling task");
                EcsEntity unit = filter.GetEntity(i);
                ref TaskComponent task = ref filter.Get2(i);
                EcsEntity taskEntity = task.task;
                TaskActionsComponent taskActions = taskEntity.Get<TaskActionsComponent>();
                int checkResult = checkAction(taskActions.getNextAction(), unit);
                switch (checkResult) {
                    case FAIL:
                        break;
                    case PERFORM:
                        // unit.Replace<MovementTargetComponent>(new MovementTargetComponent()
                        // {target = task.getNextAction().target.getPosition()});
                        break;
                    case MOVE: // set target for movement
                        unit.Replace<MovementTargetComponent>(new MovementTargetComponent { target = taskActions.getNextAction().target.getPosition() });
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