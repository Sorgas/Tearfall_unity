using System.Linq;
using game.model.component;
using game.model.component.task;
using game.model.component.unit;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using static types.action.TaskStatusEnum;

namespace game.model.container.task {
    // when task is completed, this util immediately updates all linked entities
    public class TaskCompletionUtil {
        private LocalModel model;
        public EcsFilter<TaskActionsComponent, TaskFinishedComponent> filter;
        private string logMessage;

        public void complete(EcsEntity task, TaskStatusEnum status) {
            log("[TaskCompletionUtil]: completing task " + task.Get<TaskActionsComponent>().initialAction.name);
            detachPerformer(task);
            detachDesignation(ref task, status);
            detachBuilding(ref task, status);
            detachZone(ref task);
            unlockItems(task);
            flushLog();
        }

        // if task has performer, remove task-related components from him. 
        private void detachPerformer(EcsEntity task) {
            if (!task.Has<TaskPerformerComponent>()) return;
            ref EcsEntity unit = ref task.takeRef<TaskPerformerComponent>().performer;
            // TODO perform action cancellation (drop items)
            unit.Del<UnitMovementPathComponent>();
            unit.Del<UnitMovementTargetComponent>();
            unit.Del<UnitCurrentActionComponent>();
            unit.Del<TaskComponent>();
            task.Del<TaskPerformerComponent>();
        }

        // detaches designation from
        private void detachDesignation(ref EcsEntity task, TaskStatusEnum status) {
            if (!task.Has<TaskDesignationComponent>()) return;
            EcsEntity designation = task.take<TaskDesignationComponent>().designation;
            if (status == FAILED) {
                // designation without TaskComponent will recreate task, see DesignationTaskCreationSystem
                designation.Del<TaskComponent>();
            } else {
                // complete or canceled tasks should remove designation
                model.designationContainer.removeDesignation(designation.pos());
            }
            task.Del<TaskDesignationComponent>();
        }

        // notify building that task was completed by unit
        private void detachBuilding(ref EcsEntity task, TaskStatusEnum status) {
            if (status == COMPLETE && task.Has<TaskBuildingComponent>()) {
                ref EcsEntity building = ref task.takeRef<TaskBuildingComponent>().building;
                building.Replace(new TaskFinishedComponent { status = status });
                building.Del<TaskComponent>();
                log(", building detached");
            }
        }

        // on any status task is removed from zone components 
        private void detachZone(ref EcsEntity task) {
            if (!task.Has<TaskZoneComponent>()) return;
            EcsEntity zone = task.take<TaskZoneComponent>().zone;
            if (!zone.IsAlive()) return; // zone is deleted
            unlockTiles(task, zone);
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            tracking.totalTasks.Remove(task);
            foreach (Vector3Int tile in tracking.locked.Keys) {
                if (tracking.locked[tile] == task) {
                    Debug.LogError("zone tile " + tile + " is locked by completed task " + task.name());
                }
            }
            if (zone.Has<StockpileOpenStoreTaskComponent>() && zone.take<StockpileOpenStoreTaskComponent>().bringTask.Equals(task)) {
                zone.Del<StockpileOpenStoreTaskComponent>();
            } else if (zone.Has<StockpileOpenRemoveTaskComponent>() && zone.take<StockpileOpenRemoveTaskComponent>().removeTask.Equals(task)) {
                zone.Del<StockpileOpenRemoveTaskComponent>();
            } else if (zone.Has<FarmOpenHoeingTaskComponent>() && zone.take<FarmOpenHoeingTaskComponent>().hoeTask.Equals(task)) {
                zone.Del<FarmOpenHoeingTaskComponent>();
            } else if (zone.Has<FarmOpenPlantingTaskComponent>() && zone.take<FarmOpenPlantingTaskComponent>().plantTask.Equals(task)) {
                zone.Del<FarmOpenPlantingTaskComponent>();
            } else if (zone.Has<FarmOpenRemovingTaskComponent>() && zone.take<FarmOpenRemovingTaskComponent>().removeTask.Equals(task)) {
                zone.Del<FarmOpenRemovingTaskComponent>();
            } else if (zone.Has<FarmOpenHarvestTaskComponent>() && zone.take<FarmOpenHarvestTaskComponent>().harvestTask.Equals(task)) {
                zone.Del<FarmOpenHarvestTaskComponent>();
            } else if (zone.Has<FarmOpenTaskComponent>() && zone.take<FarmOpenTaskComponent>().task.Equals(task)) {
                zone.Del<FarmOpenTaskComponent>();
                log("farm open task deleted");
            }
        }

        // unlock all items locked by task
        private void unlockItems(EcsEntity task) {
            if (task.Has<TaskLockedItemsComponent>()) {
                TaskLockedItemsComponent lockedComponent = task.take<TaskLockedItemsComponent>();
                foreach (EcsEntity item in lockedComponent.lockedItems) {
                    if (item.IsAlive()) item.Del<LockedComponent>();
                }
                log(", unlocked " + lockedComponent.lockedItems.Count + " items");
            }
        }

        private void unlockTiles(EcsEntity task, EcsEntity zone) {
            if (!zone.IsAlive()) return;
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            tracking.locked
                .Where(pair => pair.Value == task)
                .Select(pair => pair.Key)
                .ToList() // TODO required ?
                .ForEach(tile => tracking.locked.Remove(tile));
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