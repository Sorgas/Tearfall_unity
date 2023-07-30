using game.model.component.task;
using Leopotam.Ecs;

namespace game.model.system.task {
// rolls delay time of tasks, releases tasks with elapsed delays to open tasks.
public class TaskTimeoutSystem  : LocalModelScalableEcsSystem {
    public EcsFilter<TaskActionsComponent, TaskTimeoutComponent> filter;
    
    protected override void runLogic(int ticks) {
        foreach (var i in filter) {
            EcsEntity task = filter.GetEntity(i);
            ref TaskTimeoutComponent timeoutComponent = ref filter.Get2(i);
            timeoutComponent.timeout -= ticks;
            if (timeoutComponent.timeout <= 0) {
                model.taskContainer.moveDelayedTaskToOpen(task);
            }
        }
    }
}
}