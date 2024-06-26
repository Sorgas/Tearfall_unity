using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util;
using util.lang.extension;
using static types.action.ActionCheckingEnum;
using static types.action.TaskStatusEnum;
using static game.model.component.task.action.target.ActionTargetStatusEnum;

namespace game.model.system.unit {
    // handle units with tasks, which are not moving or performing
    // if task is finished, mark unit as completed task. see UnitTaskCompletionSystem
    // if action of task is finished, remove this action from task
    // check action condition(tools, ingredients, etc.) (can create sub-actions or fail task)
    // check action target availability (can add target for unit movement or fail task)
    // TODO use IgnoreInFilter for flag components. See leoecs github page
    public class UnitActionCheckingSystem : LocalModelScalableEcsSystem {
        public EcsFilter<UnitComponent, TaskComponent>.Exclude<UnitMovementTargetComponent, UnitCurrentActionComponent> filter;

        public UnitActionCheckingSystem() {
            name = "UnitActionCheckingSystem";
            debug = true;
        }

        protected override void runLogic(int ticks) {
            foreach (int i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                ref TaskComponent taskComponent = ref filter.Get2(i);
                ref TaskActionsComponent task = ref taskComponent.task.takeRef<TaskActionsComponent>();
                // log("handling task " + taskComponent.task.name());
                if (checkCompletion(ref unit, ref task)) return;
                if (!actionConditionOk(ref unit, ref task, ticks)) return;
                checkTargetAvailability(unit, task, taskComponent.task);
            }
        }

        // check if next action of task is completed. remove completed action. 
        // Mark unit with completed action when initial action of task is complete. See UnitTaskCompletionSystem 
        // returns true if task(initial action is complete)
        private bool checkCompletion(ref EcsEntity unit, ref TaskActionsComponent task) {
            if (task.initialAction.status == ActionStatusEnum.COMPLETE) { // main action and task are complete, mark unit
                log("initial action completed [" + task.initialAction.name + "]");
                model.taskContainer.removeTask(unit.take<TaskComponent>().task, COMPLETE);
                return true;
            }
            // remove completed action from task
            if (task.nextAction.status == ActionStatusEnum.COMPLETE) task.removeFirstPreAction();
            return false;
        }

        // checks action start condition and create sub action if needed.
        private bool actionConditionOk(ref EcsEntity unit, ref TaskActionsComponent actions, int ticks) {
            string nextActionName = actions.nextAction.name;
            ActionCheckingEnum checkResult = actions.nextAction.startCondition.Invoke(); // creates sub actions
            if (checkResult == OK) return true; // can start performing
            if (checkResult == FAIL) {
                log("checked start condition of [" + nextActionName + "]: FAIL");
                failTask(unit); // fail task by start condition
                return false;
            }
            if (checkResult == NEW) { // will be checked on next tick
                log("checked start condition of [" + nextActionName + "]: NEW: " + actions.nextAction.name);
                return ticks > 0 && actionConditionOk(ref unit, ref actions, ticks - 1); // false on 0 ticks
            }
            throw new GameException("Unhandled ActionConditionStatusEnum value: " + checkResult);
        }

        // checks if unit can reach action's target
        private void checkTargetAvailability(EcsEntity unit, TaskActionsComponent task, EcsEntity taskEntity) {
            Action action = task.nextAction;
            string checkResult = action.target.check(unit, model);
            if (checkResult.Equals("ready")) {
                unit.Replace(new UnitCurrentActionComponent { action = action });
            } else if (checkResult.Equals("move")) {
                unit.Replace(new UnitMovementTargetComponent { target = action.target });
            } else if (checkResult.Equals("no positions")) {
                failTask(unit);
            } 
            log($"checked target of [{action.name}] for unit {unit.name()}: {checkResult}");
        }

        private void failTask(EcsEntity unit) {
            model.taskContainer.removeTask(unit.take<TaskComponent>().task, FAILED);
        }
    }
}