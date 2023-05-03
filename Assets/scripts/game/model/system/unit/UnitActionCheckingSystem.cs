using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util;
using util.lang.extension;
using static types.action.ActionConditionStatusEnum;
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
        private bool debug = true;

        protected override void runLogic(int ticks) {
            foreach (int i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                ref TaskComponent taskComponent = ref filter.Get2(i);
                ref TaskActionsComponent task = ref taskComponent.task.takeRef<TaskActionsComponent>();
                // log("handling task " + taskComponent.task.name());
                if (checkCompletion(ref unit, ref task)) return;
                if (!actionConditionOk(ref unit, ref task, ticks)) return;
                checkTargetAvailability(ref unit, task, taskComponent.task);
            }
        }

        // check if next action of task is completed. remove completed action. 
        // Mark unit with completed action when initial action of task is complete. See UnitTaskCompletionSystem 
        // returns true if task(initial action is complete)
        private bool checkCompletion(ref EcsEntity unit, ref TaskActionsComponent task) {
            if (task.initialAction.status == ActionStatusEnum.COMPLETE) { // main action and task are complete, mark unit
                log("initial action completed [" + task.initialAction.name + "]");
                unit.Replace(new TaskFinishedComponent { status = COMPLETE });
                return true;
            }
            // remove completed action from task
            if (task.NextAction.status == ActionStatusEnum.COMPLETE) task.removeFirstPreAction();
            return false;
        }

        // checks action start condition and create sub action if needed.
        private bool actionConditionOk(ref EcsEntity unit, ref TaskActionsComponent actions, int ticks) {
            string nextActionName = actions.NextAction.name;
            ActionConditionStatusEnum checkResult = actions.NextAction.startCondition.Invoke(); // creates sub actions
            if (checkResult == OK) return true; // can start performing
            if (checkResult == FAIL) {
                log("checked start condition of [" + nextActionName + "]: FAIL");
                failTask(ref unit); // fail task by start condition
                return false;
            }
            if (checkResult == NEW) { // will be checked on next tick
                log("checked start condition of [" + nextActionName + "]: NEW: " + actions.NextAction.name);
                return ticks > 0 && actionConditionOk(ref unit, ref actions, ticks - 1); // false on 0 ticks
            }
            throw new GameException("Unhandled ActionConditionStatusEnum value: " + checkResult);
        }

        // checks if unit can reach action's target
        private void checkTargetAvailability(ref EcsEntity unit, TaskActionsComponent task, EcsEntity taskEntity) {
            Action action = task.NextAction;
            string message = "";
            switch (action.target.check(unit, model)) {
                case READY: // start performing
                    message += " ready";
                    unit.Replace(new UnitCurrentActionComponent { action = action });
                    break;
                case WAIT: // start movement
                    Vector3Int? target = action.target.pos;
                    if (target == Vector3Int.back) {
                        Debug.LogError("action [" + action.name + "] has no target position.");
                        break;
                    }
                    message += " move to " + target.Value;
                    unit.Replace(new UnitMovementTargetComponent { target = target.Value, targetType = action.target.type });
                    break;
                case STEP_OFF:
                    message += " step off";
                    action.addPreAction(new StepOffAction(unit.pos(), model));
                    break;
            }
            log("checked target of [" + action.name + "] for unit " + unit.name() + "." + message);
        }

        private void failTask(ref EcsEntity unit) => unit.Replace(new TaskFinishedComponent { status = FAILED });

        private void log(string message) {
            if(debug) Debug.Log("[UnitActionCheckingSystem]: " + message);
        }
    }
}