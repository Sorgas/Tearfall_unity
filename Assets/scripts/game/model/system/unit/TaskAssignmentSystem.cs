using Leopotam.Ecs;

namespace Tearfall_unity.Assets.scripts.game.model.system.unit {
    public class TaskAssignmentSystem : IEcsRunSystem {
        EcsFilter<UnitComponent>.Exclude<TaskComponent> filter;
        // EcsFilter<TaskComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                // TODO get task from container
                TaskComponent? task = getTaskForUnit();
                // TODO add needs
                if (task == null) task = createIdleTask();
                if (task != null) unit.Replace<TaskComponent>((TaskComponent)task);
            }
        }

        // gets any task for unit
        private TaskComponent? getTaskForUnit() {
            return null; // TODO get from task container
        }

        private TaskComponent? createIdleTask() {
            return null; // TODO create wandering task or recreation task
        }
    }
}