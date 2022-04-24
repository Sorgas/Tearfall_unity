using enums.action;
using game.model.component;
using Leopotam.Ecs;
using UnityEngine;
using static game.model.component.task.TaskComponents;

namespace game.model.system.task {
    public class TaskCompletionSystem : IEcsRunSystem {
        public EcsFilter<TaskActionsComponent, TaskFinishedComponent> filter;

        public void Run() {
            foreach (var i in filter) {
                ref EcsEntity task = ref filter.GetEntity(i);
                TaskFinishedComponent component = filter.Get2(i);
                log("completing task " + task.Get<TaskActionsComponent>().initialAction.name);
                detachPerformer(ref task, component);
                detachDesignation(ref task, component);
                GameModel.get().taskContainer.removeTask(task);
            }
        }

        // unlinks unit from task and notifies unit that task is finished
        private void detachPerformer(ref EcsEntity task, TaskFinishedComponent component) {
            if (task.Has<TaskPerformerComponent>()) {
                log("detaching performer");
                ref EcsEntity unit = ref task.Get<TaskPerformerComponent>().performer; 
                unit.Replace(component);
                unit.Del<TaskComponent>();
                task.Del<TaskPerformerComponent>();
            }
        }

        // if task was completed, designation is no longer needed
        private void detachDesignation(ref EcsEntity task, TaskFinishedComponent component) {
            if (component.status == TaskStatusEnum.COMPLETE && task.Has<TaskDesignationComponent>()) {
                log("detaching designation");
                ref EcsEntity designation = ref task.Get<TaskDesignationComponent>().designation;
                designation.Replace(component);
                designation.Del<TaskComponent>();
                task.Del<TaskDesignationComponent>();
            }
            // TODO handle workbenches
        }

        private void log(string message) {
            Debug.Log("[TaskCompletionSystem]: " + message);
        }
    }
}