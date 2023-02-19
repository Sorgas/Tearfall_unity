using Leopotam.Ecs;
using static game.model.component.task.TaskComponents;

namespace game.model.system.zone {
    // when task of zone is assigned to unit, it should be moved to ZoneTasksComponent for tracking
    public class ZoneTaskAssignmentSystem : IEcsRunSystem {
        public EcsFilter<TaskZoneComponent, TaskPerformerComponent> filter;
        
        public void Run() {
                        
            
        }
    }
}