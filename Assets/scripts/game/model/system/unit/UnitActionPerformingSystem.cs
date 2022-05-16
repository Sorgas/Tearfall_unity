using enums.action;
using game.model.component;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.unit {
    // runs active actions for units
    public class UnitActionPerformingSystem : IEcsRunSystem {
        public EcsFilter<UnitCurrentActionComponent>.Exclude<TaskFinishedComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                UnitCurrentActionComponent component = filter.Get1(i);
                Action action = component.action;
                Debug.Log("performing action " + action.name);
                action.perform(unit);
                if (action.status == ActionStatusEnum.COMPLETE) {
                    unit.Del<UnitCurrentActionComponent>();
                }
            }
        }
    }
}