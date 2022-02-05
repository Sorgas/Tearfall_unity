using enums.action;
using game.model.component;
using Leopotam.Ecs;
using static game.model.component.task.TaskComponents;

namespace game.model.system.task {
    // TODO task is not entity
    public class TaskCompletionSystem : IEcsRunSystem {
        public EcsFilter<TaskFinishedComponent> filter;

        public void Run() {
            foreach (var i in filter) {
                EcsEntity task = filter.GetEntity(i);
                TaskFinishedComponent component = filter.Get1(i);
                detachPerformer(task, component);
                detachDesignation(task, component);
            }
        }

        // unlinks unit from task and notifies unit that task is finished
        private void detachPerformer(EcsEntity task, TaskFinishedComponent component) {
            if (task.Has<TaskPerformerComponent>()) {
                task.Get<TaskPerformerComponent>().performer.Replace(component);
                task.Del<TaskPerformerComponent>();
            }
        }

        // if task was completed, designation is no longer needed
        private void detachDesignation(EcsEntity task, TaskFinishedComponent component) {
            if (component.status == TaskStatusEnum.COMPLETE && task.Has<TaskDesignationComponent>()) {
                // TODO handle workbenches
                task.Get<TaskDesignationComponent>().designation.Replace(component);
                task.Del<TaskDesignationComponent>();
            }
        }
    }
}