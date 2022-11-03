using enums.action;
using game.model.component;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using static enums.action.ActionConditionStatusEnum;
using static enums.action.TaskStatusEnum;
using static game.model.component.task.action.target.ActionTargetStatusEnum;
using static game.model.component.task.TaskComponents;

namespace game.model.system.unit {
    // handle units with tasks, which are not moving or performing
    // if task is finished, mark unit as completed task. see UnitTaskCompletionSystem
    // if action of task is finished, remove this action from task
    // check action condition(tools, ingredients) (can create sub-actions or fail task)
    // check action target availability (can add target for unit movement or fail task)
    // TODO use IgnoreInFilter for flag components. See leoecs github page
    public class UnitActionCheckingSystem : LocalModelEcsSystem {
        public EcsFilter<UnitComponent, TaskComponent>.Exclude<UnitMovementTargetComponent, UnitCurrentActionComponent> filter;

        public UnitActionCheckingSystem(LocalModel model) : base(model) { }

        public override void Run() {
            foreach (int i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                ref TaskComponent taskComponent = ref filter.Get2(i);
                ref TaskActionsComponent task = ref taskComponent.task.takeRef<TaskActionsComponent>();
                log("handling task " + taskComponent.task.name());
                if (checkCompletion(ref unit, ref task)) return;
                if (!actionConditionOk(ref unit, ref task)) return;
                checkTargetAvailability(ref unit, task, taskComponent.task);
            }
        }

        // check if next action of task is complete. remove completed action. 
        // Mark unit with completed action when initial action of task is complete. See UnitTaskCompletionSystem 
        // returns true if task(initial action is complete)
        private bool checkCompletion(ref EcsEntity unit, ref TaskActionsComponent task) {
            if (task.initialAction.status == ActionStatusEnum.COMPLETE) { // main action and task are complete, mark unit
                log("initial action completed " + task.initialAction.name);
                unit.Replace(new TaskFinishedComponent { status = COMPLETE });
                return true;
            }
            if (task.NextAction.status == ActionStatusEnum.COMPLETE) { // roll to next action when current is complete
                task.removeFirstPreAction();
            }
            return false;
        }

        // checks action start condition and create sub action if needed. Created sub action handled on next tick
        private bool actionConditionOk(ref EcsEntity unit, ref TaskActionsComponent actions) {
            string nextActionName = actions.NextAction.name;
            ActionConditionStatusEnum checkResult = actions.NextAction.startCondition.Invoke(); // creates sub actions
            log("checking action start condition " + nextActionName + ":" + checkResult);
            if (checkResult == OK) return true;
            if (checkResult == ActionConditionStatusEnum.FAIL) failTask(ref unit); // fail task by start condition
            return false; // NEW
        }

        // checks if unit can reach action's target
        private void checkTargetAvailability(ref EcsEntity unit, TaskActionsComponent task, EcsEntity taskEntity) {
            Action action = task.NextAction;
            string message = "checking action target of action " + action.name + " for unit " + unit.name() + ".";
            switch (action.target.check(unit, model)) {
                case READY: // start performing
                    message += " ready";
                    unit.Replace(new UnitCurrentActionComponent { action = action });
                    break;
                case WAIT: // start movement
                    Vector3Int? target = action.target.Pos;
                    if (!target.HasValue) {
                        Debug.LogWarning("action " + action + " has not target position.");
                        break;
                    }
                    message += " move to " + target.Value;
                    unit.Replace(new UnitMovementTargetComponent { target = target.Value, targetType = action.target.type });
                    break;
                case STEP_OFF:
                    message += " step off";
                    action.addPreAction(new StepOffAction(unit.pos(), model));
                    break;
                case ActionTargetStatusEnum.FAIL:
                    message += " target failed";
                    failTask(ref unit); // fail task with unreachable target
                    break;
            }
            log(message);
        }

        private void failTask(ref EcsEntity unit) {
            log("task failed");
            unit.Replace(new TaskFinishedComponent { status = FAILED });
        }

        private void log(string message) {
            Debug.Log("[UnitActionCheckingSystem]: " + message);
        }
    }
}