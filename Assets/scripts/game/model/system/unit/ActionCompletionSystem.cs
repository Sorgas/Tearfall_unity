using game.model.component.unit;
using Leopotam.Ecs;
using static game.model.component.task.TaskComponents;

namespace game.model.system.unit {
    // handles units which completed or failed tasks
    // detaches task from unit
    // resets and reopens failed tasks
    // TODO give exp
    public class ActionCompletionSystem : IEcsRunSystem {
        public EcsFilter<UnitComponent, TaskStatusComponent> filter;
        
        public void Run() {
            foreach (var i in filter) {
                
            }
        }
    }
}