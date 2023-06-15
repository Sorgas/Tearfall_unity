using game.model.component;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;

namespace game.model.system.unit {
    // runs active actions for units
    public class UnitActionPerformingSystem : LocalModelScalableEcsSystem {
        public EcsFilter<UnitCurrentActionComponent>.Exclude<TaskFinishedComponent> filter;

        protected override void runLogic(int ticks) {
            foreach (int i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                UnitCurrentActionComponent component = filter.Get1(i);
                Action action = component.action;
                // Debug.Log("[UnitActionPerformingSystem]: performing action " + action.name);
                action.perform(ticks);
                // Debug.Log("[UnitActionPerformingSystem]: task components count: " + action.task.GetComponentsCount());
                if (action.status == ActionStatusEnum.COMPLETE) {
                    Debug.Log("[UnitActionPerformingSystem]: action " + action.name + " complete");
                    // Debug.Log("[UnitActionPerformingSystem]: components count: " + unit.GetComponentsCount());
                    unit.Del<UnitCurrentActionComponent>();
                }
            }
        }
    }
}