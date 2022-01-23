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
    public class ActionCheckingSystem : IEcsRunSystem {
        public EcsFilter<UnitComponent, TaskComponent>.Exclude<MovementTargetComponent, CurrentActionComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                Debug.Log("handling task");
                ref TaskComponent taskComponent = ref filter.Get2(i);
                ref TaskActionsComponent task = ref taskComponent.task.Get<TaskActionsComponent>();
                EcsEntity unit = filter.GetEntity(i);
                if (checkCompletion(unit, task)) return;
                if (checkActionCondition(unit, task)) return;
                checkTargetAvailability(unit, task);
            }
        }

        private bool checkCompletion(EcsEntity unit, TaskActionsComponent task) {
            if (task.initialAction.status == ActionStatusEnum.COMPLETE) { // main action and task are complete
                Debug.Log("completing task");
                unit.Replace(new TaskStatusComponent { status = COMPLETE });
                return true;
            }
            if (task.preActions.Count > 0 && task.getNextAction().status == ActionStatusEnum.COMPLETE) { // roll to next action when current is complete
                task.removeFirstPreAction();
            }
            return false;
        }

        // checks action start condition. Created sub action handled on next tick
        private bool checkActionCondition(EcsEntity unit, TaskActionsComponent actions) {
            ActionConditionStatusEnum checkResult = actions.getNextAction().startCondition.Invoke(unit);
            if (checkResult == OK) {
                return false;
            }
            if (checkResult == ActionConditionStatusEnum.FAIL) {
                failTask(unit); // fail task by start condition
            }
            return true;
        }

        // checks if unit can reach action's target
        private void checkTargetAvailability(EcsEntity unit, TaskActionsComponent task) {
            Action action = task.getNextAction();
            Debug.Log("checking action target of action " + action + " for unit " + unit);
            switch (action.target.check(unit)) {
                case READY: // start performing
                    Debug.Log("ready");
                    unit.Replace(new CurrentActionComponent { action = action });
                    break;
                case WAIT: // start movement
                    Debug.Log("move to " + action.target.getPosition());
                    unit.Replace(new MovementTargetComponent { target = action.target.getPosition(), targetType = action.target.type });
                    break;
                case STEP_OFF:
                    Debug.Log("step off");
                    // TODO add stepoff action
                    // task.addFirstPreAction(new StepOffAction());
                    break;
                case ActionTargetStatusEnum.FAIL:
                    failTask(unit); // fail task with unreachable target
                    break;
            }
        }

        private void failTask(EcsEntity unit) {
            Debug.Log("task failed");
            unit.Replace(new TaskStatusComponent { status = FAILED });
        }
    }
}