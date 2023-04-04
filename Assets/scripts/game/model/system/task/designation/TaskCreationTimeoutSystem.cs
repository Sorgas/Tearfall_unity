using game.model.component;
using Leopotam.Ecs;

namespace game.model.system.task.designation {
    public class TaskCreationTimeoutSystem : LocalModelScalableEcsSystem {
        public EcsFilter<TaskCreationTimeoutComponent> filter;

        protected override void runLogic(int ticks) {
            foreach (var i in filter) {
                ref TaskCreationTimeoutComponent timeout = ref filter.Get1(i);
                timeout.value -= ticks;
                if (timeout.value <= 0) {
                    filter.GetEntity(i).Del<TaskCreationTimeoutComponent>();
                }
            }
        }
    }
}