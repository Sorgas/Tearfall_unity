using enums.action;
using game.model.component;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using static enums.action.ActionConditionStatusEnum;
using static enums.action.TaskStatusEnum;
using static game.model.component.task.action.target.ActionTargetStatusEnum;
using static game.model.component.task.TaskComponents;

namespace game.model.system.unit {
    // handle units with tasks, which are not moving or performing
    // if task is finished, mark unit as completed task. see UnitTaskCompletionSystem
    // if not initial action of task is finished, remove this action from task
    // check action condition and target availability
    // create sub actions if needed
    public class UnitActionCheckingSystem : IEcsRunSystem {
        public EcsFilter<UnitComponent, TaskComponent>.Exclude<UnitMovementTargetComponent, UnitCurrentActionComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                Debug.Log("handling task");
                ref TaskComponent taskComponent = ref filter.Get2(i);
                ref TaskActionsComponent task = ref taskComponent.task.Get<TaskActionsComponent>();
                ref EcsEntity unit = ref filter.GetEntity(i);
                if (checkCompletion(ref unit, ref task)) return;
                if (checkActionCondition(ref unit, ref task)) return;
                checkTargetAvailability(ref unit, task);
            }
        }

        // check if next action of task is complete. remove complete action. Mark task as completed if initial action is complete
        private bool checkCompletion(ref EcsEntity unit, ref TaskActionsComponent task) {
            if (task.initialAction.status == ActionStatusEnum.COMPLETE) { // main action and task are complete
                Debug.Log("completing task");
                unit.Replace(new TaskFinishedComponent { status = COMPLETE });
                return true;
            }
            if (task.preActions.Count > 0 && task.getNextAction().status == ActionStatusEnum.COMPLETE) { // roll to next action when current is complete
                task.removeFirstPreAction();
            }
            return false;
        }

        // checks action start condition and create sub action if needed. Created sub action handled on next tick
        private bool checkActionCondition(ref EcsEntity unit, ref TaskActionsComponent actions) {
            Debug.Log("checking action " + actions.getNextAction().name);
            ActionConditionStatusEnum checkResult = actions.getNextAction().startCondition.Invoke(); // creates sub actions
            if (checkResult == OK) {
                return false;
            }
            if (checkResult == ActionConditionStatusEnum.FAIL) {
                failTask(ref unit); // fail task by start condition
            }
            return true;
        }

        // checks if unit can reach action's target
        private void checkTargetAvailability(ref EcsEntity unit, TaskActionsComponent task) {
            Action action = task.getNextAction();
            Debug.Log("checking action target of action " + action.name + " for unit " + unit);
            switch (action.target.check(unit)) {
                case READY: // start performing
                    Debug.Log("ready");
                    unit.Replace(new UnitCurrentActionComponent { action = action });
                    break;
                case WAIT: // start movement
                    Vector3Int? target = action.target.getPos();
                    if (!target.HasValue) {
                        Debug.LogWarning("action " + action + " has not target position.");
                        break;
                    }
                    Debug.Log("move to " + target.Value);
                    unit.Replace(new UnitMovementTargetComponent { target = target.Value, targetType = action.target.type });
                    break;
                case STEP_OFF:
                    Debug.Log("step off");
                    // TODO add stepoff action
                    // task.addFirstPreAction(new StepOffAction());
                    break;
                case ActionTargetStatusEnum.FAIL:
                    failTask(ref unit); // fail task with unreachable target
                    break;
            }
        }

        private void failTask(ref EcsEntity unit) {
            Debug.Log("task failed");
            unit.Replace(new TaskFinishedComponent { status = FAILED });
        }
    }
}