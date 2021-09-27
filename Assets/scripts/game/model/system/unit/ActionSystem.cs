using Leopotam.Ecs;

namespace Tearfall_unity.Assets.scripts.game.model.system.unit {
    public class ActionSystem : IEcsRunSystem {
        public EcsFilter<TaskComponent>.Exclude<MovementTargetComponent, CurrentActionComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                handleTask(filter.Get1(i), filter.GetEntity(i));
            }
        }

        // creates action sequence in a task
        private void handleTask(TaskComponent task, EcsEntity unit) {
            if(task.getNextAction().status == ActionStatusEnum.COMPLETE) {
                task.removeFirstPreAction();
            }
            switch (task.getNextAction().startCondition.Invoke(unit)) {
                case ActionConditionStatusEnum.OK:
                    checkActionTarget(task.getNextAction(), unit);
                    break;
                case ActionConditionStatusEnum.FAIL:
                    unit.Del<TaskComponent>(); // remove failed task from unit
                    break;
            }
        }

        // check unit's position against action target
        private void checkActionTarget(_Action action, EcsEntity unit) {
            switch (action.target.check(unit)) {
                case ActionTargetStatusEnum.READY: // start performing
                    action.status = ActionStatusEnum.ACTIVE;
                    unit.Replace<CurrentActionComponent>(new CurrentActionComponent() { action = action });
                    break;
                case ActionTargetStatusEnum.WAIT: // start movement
                    action.status = ActionStatusEnum.ACTIVE;
                    unit.Replace<MovementTargetComponent>(new MovementTargetComponent() { target = action.target.getPosition() });
                    break;
                case ActionTargetStatusEnum.STEP_OFF:
                    // TODO add stepoff action
                    break;
                case ActionTargetStatusEnum.FAIL:
                    unit.Del<TaskComponent>(); // remove failed task from unit
                    break;
            }
        }
    }
}