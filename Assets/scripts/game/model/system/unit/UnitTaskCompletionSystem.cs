using enums.action;
using game.model.component;
using game.model.component.task;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.unit {
    // When unit fails or completes task, it gets UnitTaskFinishedComponent. see UnitActionCheckingSystem.
    // This system removes task component from unit and adds TaskFinishedComponent to task.
    // All task cases then handled in TaskCompletionSystem
    // TODO give exp
    public class UnitTaskCompletionSystem : IEcsRunSystem {
        public EcsFilter<UnitComponent, TaskFinishedComponent> filter;

        public void Run() {
            foreach (var i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                TaskStatusEnum status = filter.Get2(i).status;
                Debug.Log("[UnitTaskCompletionSystem]: completing task for unit " + unit.Get<UnitNameComponent>().name);
                if (status == TaskStatusEnum.FAILED || status == TaskStatusEnum.CANCELED) {
                    unit.Del<UnitMovementPathComponent>();
                    unit.Del<UnitMovementTargetComponent>();
                    unit.Del<UnitCurrentActionComponent>();
                    // TODO handle task cancelling and interruption (do cancel effects of action(drop item, etc))
                }
                detachTaskFromUnit(ref unit, status);       
                unit.Del<TaskFinishedComponent>();
            }
        }
        
        private void detachTaskFromUnit(ref EcsEntity unit, TaskStatusEnum status) {
            if (unit.Has<TaskComponent>()) {
                EcsEntity task = unit.takeRef<TaskComponent>().task;
                task.Del<TaskComponents.TaskPerformerComponent>();
                task.Replace(new TaskFinishedComponent {status = status}); // move status to task, it will be handled by TaskStatusSystem
                unit.Del<TaskComponent>();
            }
        }
    }
}