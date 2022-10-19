using enums.action;
using game.model.component;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

namespace game.model.system.task {
    // When task is completed by some reason, this system notifies other entities related to task.
    public class TaskCompletionSystem : LocalModelEcsSystem {
        public EcsFilter<TaskActionsComponent, TaskFinishedComponent> filter;

        public TaskCompletionSystem(LocalModel model) : base(model) { }

        public override void Run() {
            foreach (var i in filter) {
                ref EcsEntity task = ref filter.GetEntity(i);
                TaskFinishedComponent component = filter.Get2(i);
                log("completing task " + task.Get<TaskActionsComponent>().initialAction.name);
                detachPerformer(ref task, component);
                detachDesignation(ref task, component);
                detachBuilding(ref task, component);
                model.taskContainer.removeTask(task);
            }
        }

        // if task is canceled by deleting designation or order in workbench, 
        // unlink unit from task and notify that task is finished
        private void detachPerformer(ref EcsEntity task, TaskFinishedComponent component) {
            if (task.Has<TaskPerformerComponent>()) {
                log("detaching performer");
                ref EcsEntity unit = ref task.takeRef<TaskPerformerComponent>().performer; 
                unit.Replace(component);
                unit.Del<TaskComponent>();
            }
        }

        // if task was completed by unit, designation is no longer needed
        private void detachDesignation(ref EcsEntity task, TaskFinishedComponent component) {
            if (component.status == TaskStatusEnum.COMPLETE && task.Has<TaskDesignationComponent>()) {
                log("detaching designation");
                ref EcsEntity designation = ref task.takeRef<TaskDesignationComponent>().designation;
                designation.Replace(component);
                designation.Del<TaskComponent>();
            }
            // TODO handle workbenches
        }

        // notify building that task was completed by unit
        private void detachBuilding(ref EcsEntity task, TaskFinishedComponent component) {
            if (component.status == TaskStatusEnum.COMPLETE && task.Has<TaskBuildingComponent>()) {
                log("detaching building");
                ref EcsEntity building = ref task.takeRef<TaskBuildingComponent>().building;
                building.Replace(component);
                building.Del<TaskComponent>();
            }
            // TODO handle workbenches
        }

        private void log(string message) {
            Debug.Log("[TaskCompletionSystem]: " + message);
        }
    }
}