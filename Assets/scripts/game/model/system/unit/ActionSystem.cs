using enums.action;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.model.component.unit.components;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.unit {
    public class ActionSystem : IEcsRunSystem {
        public EcsFilter<TaskComponent>.Exclude<MovementTargetComponent, CurrentActionComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                Debug.Log("acting");
                handleTask(filter.Get1(i), filter.GetEntity(i));
            }
        }

        // creates action sequence in a task
        private void handleTask(TaskComponent task, EcsEntity unit) {
            Debug.Log("handling task " + task + " of unit " + unit);
            if(task.initialAction.status == ActionStatusEnum.COMPLETE) {
                Debug.Log("completing task");
                unit.Del<TaskComponent>(); // task complete
                return;
            }
            if(task.getNextAction() != task.initialAction && task.getNextAction().status == ActionStatusEnum.COMPLETE) {
                Debug.Log("completing action");
                task.removeFirstPreAction();
                return;
            }
            switch (task.getNextAction().startCondition.Invoke(unit)) {
                case ActionConditionStatusEnum.OK:
                    checkActionTarget(task.getNextAction(), unit);
                    break;
                case ActionConditionStatusEnum.FAIL:
                    Debug.Log("failed");
                    unit.Del<TaskComponent>(); // remove failed task from unit
                    break;
            }
        }

        // check unit's position against action target
        private void checkActionTarget(_Action action, EcsEntity unit) {
            Debug.Log("checking action target of action " + action + " for unit " + unit);
            switch (action.target.check(unit)) {
                case ActionTargetStatusEnum.READY: // start performing
                    Debug.Log("ready");
                    unit.Replace<CurrentActionComponent>(new CurrentActionComponent() { action = action });
                    break;
                case ActionTargetStatusEnum.WAIT: // start movement
                    Debug.Log("move to " + action.target.getPosition());
                    action.status = ActionStatusEnum.OPEN;
                    unit.Replace<MovementTargetComponent>(new MovementTargetComponent() { target = action.target.getPosition() });
                    break;
                case ActionTargetStatusEnum.STEP_OFF:
                    Debug.Log("step off");
                    // TODO add stepoff action
                    break;
                case ActionTargetStatusEnum.FAIL:
                    Debug.Log("failed");
                    unit.Del<TaskComponent>(); // remove failed task from unit
                    break;
            }
        }
    }
}