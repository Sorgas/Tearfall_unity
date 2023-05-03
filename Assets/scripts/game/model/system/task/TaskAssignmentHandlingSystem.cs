using game.model.component;
using game.model.component.task;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.task {
    // some entities, like farms and stockpiles, create and keep only one open task and wait until it is assigned before create another task
    // all tasks get TaskAssignedComponent on assignment.
    // this system notifies this entities that task is assigned
    public class TaskAssignmentHandlingSystem : IEcsRunSystem {
        public EcsFilter<TaskAssignedComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                EcsEntity task = filter.GetEntity(i);
                if (task.Has<TaskZoneComponent>()) handleZoneTask(task);
                task.Del<TaskAssignedComponent>();
            }
        }

        // moves task from open task component to task tracking component
        private void handleZoneTask(EcsEntity task) {
            TaskZoneComponent taskZone = task.take<TaskZoneComponent>();
            EcsEntity zone = taskZone.zone;
            switch (taskZone.taskType) {
                case ZoneTaskTypes.STORE_ITEM: 
                    zone.Del<StockpileOpenStoreTaskComponent>();
                    break;
                case ZoneTaskTypes.REMOVE_ITEM: 
                    zone.Del<StockpileOpenRemoveTaskComponent>();
                    break;
                case ZoneTaskTypes.HOE: 
                    zone.Del<FarmOpenHoeingTaskComponent>();
                    break;
                case ZoneTaskTypes.PLANT: 
                    zone.Del<FarmOpenPlantingTaskComponent>();
                    break;
                case ZoneTaskTypes.REMOVE_PLANT: 
                    zone.Del<FarmOpenRemovingTaskComponent>();
                    break;
                case ZoneTaskTypes.HARVEST: 
                    zone.Del<FarmOpenHarvestTaskComponent>();
                    break;
            }
        }
    }
}