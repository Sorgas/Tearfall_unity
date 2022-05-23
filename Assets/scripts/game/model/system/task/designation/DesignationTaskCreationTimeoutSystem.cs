using game.model.component;
using game.model.component.task;
using Leopotam.Ecs;

namespace game.model.system.task.designation {
    public class DesignationTaskCreationTimeoutSystem : IEcsRunSystem {
        public EcsFilter<DesignationComponent, TaskCreationTimeoutComponent> filter;
        
        public void Run() {
            foreach (var i in filter) {
                ref TaskCreationTimeoutComponent timeout = ref filter.Get2(i);
                if (timeout.value-- <= 0) {
                    filter.GetEntity(i).Del<TaskCreationTimeoutComponent>();
                }
            }
        }
    }
}