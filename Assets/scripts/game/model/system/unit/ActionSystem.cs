using enums.action;
using game.model.component;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static enums.action.ActionConditionStatusEnum;
using static enums.action.ActionStatusEnum;
using static game.model.component.task.action.target.ActionTargetStatusEnum;
using static game.model.component.task.TaskComponents;

namespace game.model.system.unit {
    // handles assigned tasks on units. creates pre actions, initiates movement for unit.
    public class ActionSystem : IEcsRunSystem {
        public EcsFilter<UnitComponent, TaskComponent>.Exclude<MovementTargetComponent, CurrentActionComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                Debug.Log("acting");
                TaskComponent task = filter.Get2(i);
                EcsEntity unit = filter.GetEntity(i); 
                handleTask(task.task.Get<TaskActionsComponent>(), unit);
            }
        }

        // creates action sequence in a task
        private void handleTask(TaskActionsComponent task, EcsEntity unit) {
            Debug.Log("handling task " + task + " of unit " + unit);
            if (task.initialAction.status == COMPLETE) { // main action and task are complete
                completeTask(task, unit);
            } else if (task.getNextAction() != task.initialAction && task.getNextAction().status == COMPLETE) { // roll to next action when current is complete
                completeAction(task);
            } else { 
                switch (task.getNextAction().startCondition.Invoke(unit)) {
                    // start condition can create new pre action
                    case OK:
                        checkActionTarget(task.getNextAction(), unit);
                        break;
                    case ActionConditionStatusEnum.FAIL:
                        failTask(unit);
                        break;
                }
            }
        }

        // check unit's position against action target
        private void checkActionTarget(Action action, EcsEntity unit) {
            Debug.Log("checking action target of action " + action + " for unit " + unit);
            switch (action.target.check(unit)) {
                case READY: // start performing
                    Debug.Log("ready");
                    unit.Replace(new CurrentActionComponent { action = action });
                    break;
                case WAIT: // start movement
                    Debug.Log("move to " + action.target.getPosition());
                    action.status = OPEN;
                    unit.Replace(new MovementTargetComponent { target = action.target.getPosition() });
                    break;
                case STEP_OFF:
                    Debug.Log("step off");
                    // TODO add stepoff action
                    break;
                case ActionTargetStatusEnum.FAIL:
                    failTask(unit);
                    break;
            }
        }

        private void completeTask(TaskActionsComponent task, EcsEntity unit) {
            Debug.Log("completing task");
            unit.Del<TaskComponent>(); // task complete
            // TODO remove from container
        }

        private void completeAction(TaskActionsComponent task) {
            Debug.Log("completing action");
            task.removeFirstPreAction();
        }

        private void failTask(EcsEntity unit) {
            Debug.Log("failed");
            unit.Del<TaskComponent>(); // remove failed task from unit
        }
    }
}