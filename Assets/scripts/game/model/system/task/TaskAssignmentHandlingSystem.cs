using game.model.component;
using game.model.component.task;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.system.task {
    public class TaskAssignmentHandlingSystem : IEcsRunSystem {
        public EcsFilter<TaskAssignedComponent> filter;
        
        public void Run() {
            foreach (int i in filter) {
                EcsEntity task = filter.GetEntity(i);
                if(task.Has<TaskZoneComponent>()) handleZoneTask(task);
                task.Del<TaskAssignedComponent>();
            }
        }

        // for stockpiles, moves task from open task component to task tracking component
        private void handleZoneTask(EcsEntity task) {
            EcsEntity zone = task.take<TaskZoneComponent>().zone;
            if (zone.Has<StockpileComponent>()) {
                if (zone.Has<StockpileOpenBringTaskComponent>() && zone.take<StockpileOpenBringTaskComponent>().bringTask == task) {
                    zone.take<StockpileTasksComponent>().bringTasks.Add(task);
                    zone.Del<StockpileOpenBringTaskComponent>();
                    return;
                }
                if (zone.Has<StockpileOpenBringTaskComponent>() && zone.take<StockpileOpenBringTaskComponent>().bringTask == task) {
                    zone.take<StockpileTasksComponent>().bringTasks.Add(task);
                    zone.Del<StockpileOpenBringTaskComponent>();
                }
            }
        }
    }
}