using game.model.component;
using game.model.component.item;
using game.model.component.task.action;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

namespace game.model.system.task {
    // When task is completed by some reason, this system notifies other entities related to task.
    public class TaskCompletionSystem : LocalModelEcsSystem {
        public EcsFilter<TaskActionsComponent, TaskFinishedComponent> filter;
        private string logMessage;

        public TaskCompletionSystem(LocalModel model) : base(model) { }

        public override void Run() {
            foreach (var i in filter) {
                ref EcsEntity task = ref filter.GetEntity(i);
                TaskFinishedComponent component = filter.Get2(i);
                log("[TaskCompletionSystem]: completing task " + task.Get<TaskActionsComponent>().initialAction.name);
                detachPerformer(ref task, component);
                detachDesignation(ref task, component);
                detachBuilding(ref task, component);
                unlockItems(task);
                flushLog();
                model.taskContainer.removeTask(task);
            }
        }

        // if task is canceled by deleting designation or order in workbench, 
        // unlink unit from task and notify that task is finished
        private void detachPerformer(ref EcsEntity task, TaskFinishedComponent component) {
            if (task.Has<TaskPerformerComponent>()) {
                ref EcsEntity unit = ref task.takeRef<TaskPerformerComponent>().performer; 
                unit.Replace(component);
                unit.Del<TaskComponent>();
                log(", performer detached");
            }
        }

        // detaches designation from 
        private void detachDesignation(ref EcsEntity task, TaskFinishedComponent component) {
            if (task.Has<TaskDesignationComponent>()) {
                ref EcsEntity designation = ref task.takeRef<TaskDesignationComponent>().designation;
                designation.Replace(component);
                designation.Del<TaskComponent>();
                log(", designation detached");
            }
        }

        // notify building that task was completed by unit
        private void detachBuilding(ref EcsEntity task, TaskFinishedComponent component) {
            if (component.status == TaskStatusEnum.COMPLETE && task.Has<TaskBuildingComponent>()) {
                ref EcsEntity building = ref task.takeRef<TaskBuildingComponent>().building;
                building.Replace(component);
                building.Del<TaskComponent>();
                log(", building detached");
            }
        }

        // unlock all items locked by task
        private void unlockItems(EcsEntity task) {

            if(task.Has<TaskLockedItemsComponent>()) {
                Action initialAction = task.take<TaskActionsComponent>().initialAction;
                TaskLockedItemsComponent lockedComponent = task.take<TaskLockedItemsComponent>();
                foreach(EcsEntity item in lockedComponent.lockedItems) {
                    if(item.IsAlive()) item.Del<LockedComponent>();
                }
                log(", unlocked " + lockedComponent.lockedItems.Count + " items");
            }
        }

        private void log(string message) {
            logMessage += message;
        }

        private void flushLog() {
            Debug.Log(logMessage);
            logMessage = "";
        }
    }
}