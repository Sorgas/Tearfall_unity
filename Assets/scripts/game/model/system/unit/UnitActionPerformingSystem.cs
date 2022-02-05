using enums.action;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.unit {
    // runs active actions for units
    public class UnitActionPerformingSystem : IEcsRunSystem {
        public EcsFilter<CurrentActionComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                Debug.Log("performing");
                EcsEntity unit = filter.GetEntity(i);
                CurrentActionComponent component = filter.Get1(i);
                Action action = component.action;
                action.perform(unit);
                if(action.status == ActionStatusEnum.COMPLETE) {
                    unit.Del<CurrentActionComponent>();
                }
            }
        }
    }
}