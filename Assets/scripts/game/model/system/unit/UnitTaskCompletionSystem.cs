using enums.action;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.unit {
    // When unit fails or completes task, it gets UnitTaskFinishedComponent. see UnitActionCheckingSystem.
    // This system removes task component from unit and adds TaskFinishedComponent to task.
    // All task cases then handled in TaskCompletionSystem
    // TODO give exp
    public class UnitTaskCompletionSystem : IEcsRunSystem {
        public EcsFilter<UnitComponent, TaskFinishedComponent> filter;

        public void Run() {
            foreach (var i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                TaskFinishedComponent finishComponent = filter.Get2(i);
                if (unit.Has<TaskComponent>() ) {
                    if (finishComponent.status == TaskStatusEnum.FAILED || finishComponent.status == TaskStatusEnum.CANCELED) {
                        // TODO handle task cancelling and interruption (do cancel effects of action(drop item, etc))
                    }
                    detachTask(unit, unit.Get<TaskComponent>().task, finishComponent);
                } else {
                    Debug.LogError("Invalid unit state: no TaskComponent and UnitTaskFinishedComponent");
                }
            }
        }

        private void detachTask(EcsEntity unit, EcsEntity task, TaskFinishedComponent finishComponent) {
            unit.Del<TaskFinishedComponent>();
            unit.Del<TaskComponent>();
            task.Replace(new TaskFinishedComponent {status = finishComponent.status}); // move status to task, it will be handled by TaskStatusSystem
        }
    }
}