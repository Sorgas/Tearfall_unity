using game.model.component;
using game.model.component.task;
using game.model.component.unit;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.unit {
    // When unit fails or completes task, it gets UnitTaskFinishedComponent. see UnitActionCheckingSystem.
    // This system removes task component from unit and adds TaskFinishedComponent to task.
    // All task cases then handled in TaskCompletionSystem
    // TODO give exp
    public class UnitTaskCompletionSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<UnitComponent, TaskFinishedComponent> filter;

        public override void Run() {
            foreach (var i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                TaskStatusEnum status = filter.Get2(i).status;
                Debug.Log("[UnitTaskCompletionSystem]: completing task for unit " + unit.Get<NameComponent>().name);
                detachTaskFromUnit(ref unit, status);       
                unit.Del<TaskFinishedComponent>();
            }
        }
        
        private void detachTaskFromUnit(ref EcsEntity unit, TaskStatusEnum status) {
        }

    }
}